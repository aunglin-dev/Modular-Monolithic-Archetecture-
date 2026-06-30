using LoginSolution.Shared.Domain.Models.Accounts;
using LoginSolution.Shared.Domain.Models.Authentication;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.IBanking.Mvc.Services.Interfaces;

public interface IIBankingApiService
{
    Task<LoginResponseModel?> LoginAsync(LoginRequestModel model, CancellationToken cancellationToken = default);
    Task<UserResponseModel?> GetProfileAsync(CancellationToken cancellationToken = default);
    Task<AccountResponseModel?> GetAccountAsync(CancellationToken cancellationToken = default);
    Task<AccountBalanceModel?> GetBalanceAsync(CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken = default);
}
