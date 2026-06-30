using System.Security.Claims;
using LoginSolution.IBanking.Mvc.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.IBanking.Mvc.Controllers;

public sealed class AccountController : Controller
{
    private readonly IIBankingApiService _api; private readonly IWebHostEnvironment _env;
    public AccountController(IIBankingApiService api, IWebHostEnvironment env) { _api = api; _env = env; }
    [HttpGet] public IActionResult Login() { ViewBag.ShowDemo = _env.IsDevelopment(); return View(new LoginRequestModel()); }
    [HttpPost] public async Task<IActionResult> Login(LoginRequestModel model, CancellationToken ct) { if (!ModelState.IsValid) { ViewBag.ShowDemo = _env.IsDevelopment(); return View(model); } var login = await _api.LoginAsync(model, ct); if (login is null || !login.IsSuccess) { ModelState.AddModelError(string.Empty, "Login failed."); ViewBag.ShowDemo = _env.IsDevelopment(); return View(model); } HttpContext.Session.SetString("AccessToken", login.AccessToken); var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, login.UserId.ToString()), new(ClaimTypes.Name, login.Username), new(ClaimTypes.Email, login.Email) }; claims.AddRange(login.Roles.Select(r => new Claim(ClaimTypes.Role, r))); await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme))); return RedirectToAction("Index", "Profile"); }
    [Authorize][HttpPost] public async Task<IActionResult> Logout() { HttpContext.Session.Clear(); await HttpContext.SignOutAsync(); return RedirectToAction("Index", "Home"); }
    public IActionResult AccessDenied() => View();
}
