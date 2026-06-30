using LoginSolution.Admin.Api.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.Admin.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/roles")]
public sealed class RoleController : ControllerBase
{
    private readonly IRoleManagementService _service; public RoleController(IRoleManagementService service) => _service = service;
    [HttpGet] public async Task<ActionResult<ApiResponseModel<IReadOnlyList<RoleResponseModel>>>> Get(CancellationToken ct) => Ok(ApiResponseModel<IReadOnlyList<RoleResponseModel>>.Success(await _service.GetRolesAsync(ct)));
    [HttpGet("{id:int}")] public async Task<ActionResult<ApiResponseModel<RoleResponseModel>>> Get(int id, CancellationToken ct) => Ok(ApiResponseModel<RoleResponseModel>.Success(await _service.GetRoleAsync(id, ct)));
    [HttpPost] public async Task<ActionResult<ApiResponseModel<RoleResponseModel>>> Post(CreateRoleModel model, CancellationToken ct) => Ok(ApiResponseModel<RoleResponseModel>.Success(await _service.CreateRoleAsync(model, ct), "Role created."));
    [HttpPut("{id:int}")] public async Task<ActionResult<ApiResponseModel<RoleResponseModel>>> Put(int id, CreateRoleModel model, CancellationToken ct) => Ok(ApiResponseModel<RoleResponseModel>.Success(await _service.UpdateRoleAsync(id, model, ct), "Role updated."));
}
