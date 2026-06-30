using LoginSolution.IBanking.Api.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Accounts;
using LoginSolution.Shared.Domain.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.IBanking.Api.Controllers;

[ApiController]
[Authorize(Roles = "Customer")]
[Route("api/[controller]")]
public sealed class AccountController : ControllerBase
{
    private readonly IAccountService _service; public AccountController(IAccountService service) => _service = service;
    [HttpGet] public async Task<ActionResult<ApiResponseModel<AccountResponseModel>>> Get(CancellationToken ct) => Ok(ApiResponseModel<AccountResponseModel>.Success(await _service.GetAccountAsync(int.Parse(User.FindFirst("user_id")!.Value), ct)));
    [HttpGet("balance")] public async Task<ActionResult<ApiResponseModel<AccountBalanceModel>>> Balance(CancellationToken ct) => Ok(ApiResponseModel<AccountBalanceModel>.Success(await _service.GetBalanceAsync(int.Parse(User.FindFirst("user_id")!.Value), ct)));
}
