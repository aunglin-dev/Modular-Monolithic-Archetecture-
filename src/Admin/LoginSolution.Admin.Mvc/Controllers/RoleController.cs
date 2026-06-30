using LoginSolution.Admin.Mvc.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.Admin.Mvc.Controllers;

[Authorize(Roles = "Admin")]
public sealed class RoleController : Controller
{
    private readonly IAdminApiService _api; public RoleController(IAdminApiService api) => _api = api;
    public async Task<IActionResult> Index(CancellationToken ct) => View(await _api.GetRolesAsync(ct));
    [HttpGet] public IActionResult Create() => View(new CreateRoleModel());
    [HttpPost] public async Task<IActionResult> Create(CreateRoleModel model, CancellationToken ct) { if (!ModelState.IsValid) return View(model); await _api.CreateRoleAsync(model, ct); return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(int id, CancellationToken ct) { var role = await _api.GetRoleAsync(id, ct); if (role is null) return NotFound(); return View(new CreateRoleModel { Name = role.Name, Description = role.Description }); }
    [HttpPost] public async Task<IActionResult> Edit(int id, CreateRoleModel model, CancellationToken ct) { if (!ModelState.IsValid) return View(model); await _api.UpdateRoleAsync(id, model, ct); return RedirectToAction(nameof(Index)); }
}
