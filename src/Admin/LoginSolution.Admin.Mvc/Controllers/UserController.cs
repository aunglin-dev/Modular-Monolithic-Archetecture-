using LoginSolution.Admin.Mvc.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.Admin.Mvc.Controllers;

[Authorize(Roles = "Admin")]
public sealed class UserController : Controller
{
    private readonly IAdminApiService _api; public UserController(IAdminApiService api) => _api = api;
    public async Task<IActionResult> Index(CancellationToken ct) => View(await _api.GetUsersAsync(cancellationToken: ct));
    [HttpGet] public IActionResult Create() => View(new CreateUserModel { RoleName = "Customer" });
    [HttpPost] public async Task<IActionResult> Create(CreateUserModel model, CancellationToken ct) { if (!ModelState.IsValid) return View(model); await _api.CreateUserAsync(model, ct); return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(int id, CancellationToken ct) { var user = await _api.GetUserAsync(id, ct); if (user is null) return NotFound(); return View(new UpdateUserModel { Id = user.Id, Username = user.Username, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, IsActive = user.IsActive, RoleName = user.Roles.FirstOrDefault() ?? "Customer" }); }
    [HttpPost] public async Task<IActionResult> Edit(int id, UpdateUserModel model, CancellationToken ct) { if (!ModelState.IsValid) return View(model); await _api.UpdateUserAsync(id, model, ct); return RedirectToAction(nameof(Index)); }
    [HttpPost] public async Task<IActionResult> Status(int id, bool isActive, CancellationToken ct) { await _api.SetUserStatusAsync(id, isActive, ct); return RedirectToAction(nameof(Index)); }
}
