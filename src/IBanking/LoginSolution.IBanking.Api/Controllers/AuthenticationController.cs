using LoginSolution.IBanking.Api.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Authentication;
using LoginSolution.Shared.Domain.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.IBanking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _service; public AuthenticationController(IAuthenticationService service) => _service = service;
    [HttpPost("login")] public async Task<ActionResult<ApiResponseModel<LoginResponseModel>>> Login(LoginRequestModel model, CancellationToken ct) => Ok(ApiResponseModel<LoginResponseModel>.Success(await _service.LoginAsync(model, HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty, Request.Headers.UserAgent.ToString(), ct)));
    [HttpPost("refresh-token")] public async Task<ActionResult<ApiResponseModel<LoginResponseModel>>> RefreshToken(RefreshTokenModel model, CancellationToken ct) => Ok(ApiResponseModel<LoginResponseModel>.Success(await _service.RefreshTokenAsync(model, ct)));
    [Authorize(Roles = "Customer")][HttpPost("change-password")] public async Task<ActionResult<ApiResponseModel>> ChangePassword(ChangePasswordModel model, CancellationToken ct) { var id = int.Parse(User.FindFirst("user_id")!.Value); await _service.ChangePasswordAsync(id, model, ct); return Ok(ApiResponseModel.Success("Password changed.")); }
}
