using LoginSolution.IBanking.Api.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.IBanking.Api.Controllers;

[ApiController]
[Authorize(Roles = "Customer")]
[Route("api/[controller]")]
public sealed class ProfileController : ControllerBase
{
    private readonly IAccountService _service; public ProfileController(IAccountService service) => _service = service;
    [HttpGet("me")] public async Task<ActionResult<ApiResponseModel<UserResponseModel>>> Me(CancellationToken ct) => Ok(ApiResponseModel<UserResponseModel>.Success(await _service.GetProfileAsync(int.Parse(User.FindFirst("user_id")!.Value), ct)));
}
