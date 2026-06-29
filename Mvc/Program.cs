using Domain.DbContexts;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc.Repositories;
using Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();

builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

/*
 Authentication must come before Authorization.
*/
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

await SeedDatabaseAsync(app);

app.Run();

static async Task SeedDatabaseAsync(WebApplication app)
{
    using IServiceScope scope =
        app.Services.CreateScope();

    AppDbContext dbContext =
        scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

    /*
     During the first migration creation there may be
     no migrations yet.
    */
    if (!dbContext.Database.GetMigrations().Any())
    {
        return;
    }

    await dbContext.Database.MigrateAsync();

    bool userExists = await dbContext.Users.AnyAsync();

    if (userExists)
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