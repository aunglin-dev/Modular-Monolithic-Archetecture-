using LoginSolution.Admin.Api.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.Admin.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/users")]
public sealed class UserController : ControllerBase
{
    private readonly IUserManagementService _service; public UserController(IUserManagementService service) => _service = service;
    [HttpGet] public async Task<ActionResult<ApiResponseModel<UserListModel>>> Get([FromQuery] PaginationRequestModel p, CancellationToken ct) => Ok(ApiResponseModel<UserListModel>.Success(await _service.GetUsersAsync(p, ct)));
    [HttpGet("{id:int}")] public async Task<ActionResult<ApiResponseModel<UserResponseModel>>> Get(int id, CancellationToken ct) => Ok(ApiResponseModel<UserResponseModel>.Success(await _service.GetUserAsync(id, ct)));
    [HttpPost] public async Task<ActionResult<ApiResponseModel<UserResponseModel>>> Post(CreateUserModel model, CancellationToken ct) => Ok(ApiResponseModel<UserResponseModel>.Success(await _service.CreateUserAsync(model, ct), "User created."));
    [HttpPut("{id:int}")] public async Task<ActionResult<ApiResponseModel<UserResponseModel>>> Put(int id, UpdateUserModel model, CancellationToken ct) => Ok(ApiResponseModel<UserResponseModel>.Success(await _service.UpdateUserAsync(id, model, ct), "User updated."));
    [HttpPatch("{id:int}/status")] public async Task<ActionResult<ApiResponseModel>> Status(int id, [FromQuery] bool isActive, CancellationToken ct) { await _service.SetStatusAsync(id, isActive, ct); return Ok(ApiResponseModel.Success("User status updated.")); }
}
