using System.Text;
using Api.Repositories;
using Api.Services;
using Domain.DbContexts;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString =
        builder.Configuration.GetConnectionString(
            "DefaultConnection")
        ?? throw new InvalidOperationException(
            "DefaultConnection is missing.");

    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();

IConfigurationSection jwtSection =
    builder.Configuration.GetSection("Jwt");

string jwtKey =
    jwtSection["Key"]
    ?? throw new InvalidOperationException(
        "JWT key is missing.");

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        /*
         Keep the claim names as:
         name, email, role and sub.
        */
        options.MapInboundClaims = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                NameClaimType = "name",
                RoleClaimType = "role",

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7002",
                "http://localhost:5002",
                "https://localhost:7202")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BlazorPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await SeedDatabaseAsync(app);

app.Run();

static async Task SeedDatabaseAsync(WebApplication app)
{
    using IServiceScope scope =
        app.Services.CreateScope();

    AppDbContext dbContext =
        scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

    if (!dbContext.Database.GetMigrations().Any())
    {
        return;
    }

    await dbContext.Database.MigrateAsync();

    if (await dbContext.Users.AnyAsync())
    {
        return;
    }

    IPasswordHasher<User> passwordHasher =
        scope.ServiceProvider
            .GetRequiredService<IPasswordHasher<User>>();

    var user = new User
    {
        FullName = "System Administrator",
        Email = "admin@example.com",
        Role = "Admin",
        IsActive = true
    };

    user.PasswordHash =
        passwordHasher.HashPassword(
            user,
            "Pass@123");

    dbContext.Users.Add(user);

    await dbContext.SaveChangesAsync();
}