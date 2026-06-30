using LoginSolution.Admin.Mvc.Services;
using LoginSolution.Admin.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(); builder.Services.AddHttpContextAccessor(); builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => { options.LoginPath = "/Account/Login"; options.AccessDeniedPath = "/Account/AccessDenied"; });
builder.Services.AddHttpClient<IAdminApiService, AdminApiService>(client => client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7201"));
var app = builder.Build(); if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error"); app.UseHttpsRedirection(); app.UseStaticFiles(); app.UseRouting(); app.UseSession(); app.UseAuthentication(); app.UseAuthorization(); app.MapControllerRoute(name: "default", pattern: "{controller=Dashboard}/{action=Index}/{id?}"); app.Run();
