using LoginSolution.IBanking.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.IBanking.Mvc.Controllers;

[Authorize(Roles = "Customer")]
public sealed class ProfileController : Controller
{
    private readonly IIBankingApiService _api; public ProfileController(IIBankingApiService api) => _api = api;
    public async Task<IActionResult> Index(CancellationToken ct) => View(await _api.GetProfileAsync(ct));
    public async Task<IActionResult> Account(CancellationToken ct) => View(await _api.GetAccountAsync(ct));
    public async Task<IActionResult> Balance(CancellationToken ct) => View(await _api.GetBalanceAsync(ct));
    [HttpGet] public IActionResult ChangePassword() => View(new LoginSolution.Shared.Domain.Models.Authentication.ChangePasswordModel());
    [HttpPost] public async Task<IActionResult> ChangePassword(LoginSolution.Shared.Domain.Models.Authentication.ChangePasswordModel model, CancellationToken ct) { if (!ModelState.IsValid) return View(model); ViewBag.Message = await _api.ChangePasswordAsync(model, ct) ? "Password changed." : "Password change failed."; return View(new LoginSolution.Shared.Domain.Models.Authentication.ChangePasswordModel()); }
}
