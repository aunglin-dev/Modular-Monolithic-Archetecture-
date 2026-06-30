using System.Text;
using LoginSolution.Admin.Api.Middleware;
using LoginSolution.Admin.Api.Services;
using LoginSolution.Admin.Api.Services.Interfaces;
using LoginSolution.Shared.Database;
using LoginSolution.Shared.Domain.Extensions;
using LoginSolution.Shared.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing. Use user secrets for the real SQL password.");
builder.Services.AddSharedDatabase(connectionString);
builder.Services.AddDomainServiceFilters();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAdminAuthenticationService, AdminAuthenticationService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IRoleManagementService, RoleManagementService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddControllers(); builder.Services.AddEndpointsApiExplorer(); builder.Services.AddSwaggerGen();
var jwtKey = builder.Configuration["JwtSettings:SecretKey"] ?? "development-placeholder-key-change-with-user-secrets-32chars";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => { options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true, ValidateAudience = true, ValidateIssuerSigningKey = true, ValidateLifetime = true, ValidIssuer = builder.Configuration["JwtSettings:Issuer"], ValidAudience = builder.Configuration["JwtSettings:Audience"], IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) }; });
builder.Services.AddAuthorization();
var app = builder.Build(); app.UseMiddleware<ExceptionMiddleware>(); if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection(); app.UseAuthentication(); app.UseAuthorization(); app.MapControllers(); app.Run();
