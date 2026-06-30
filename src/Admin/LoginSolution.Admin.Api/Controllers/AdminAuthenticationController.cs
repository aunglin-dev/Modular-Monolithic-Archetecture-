using LoginSolution.Admin.Api.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Authentication;
using LoginSolution.Shared.Domain.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.Admin.Api.Controllers;

[ApiController]
[Route("api/admin-authentication")]
public sealed class AdminAuthenticationController : ControllerBase
{
    private readonly IAdminAuthenticationService _service; public AdminAuthenticationController(IAdminAuthenticationService service) => _service = service;
    [HttpPost("login")] public async Task<ActionResult<ApiResponseModel<LoginResponseModel>>> Login(LoginRequestModel model, CancellationToken ct) => Ok(ApiResponseModel<LoginResponseModel>.Success(await _service.LoginAsync(model, HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty, Request.Headers.UserAgent.ToString(), ct)));
}
