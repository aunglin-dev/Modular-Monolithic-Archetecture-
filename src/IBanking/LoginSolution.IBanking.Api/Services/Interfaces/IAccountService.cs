using LoginSolution.Shared.Domain.Models.Accounts;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.IBanking.Api.Services.Interfaces;

public interface IAccountService
{
    Task<UserResponseModel> GetProfileAsync(int userId, CancellationToken cancellationToken = default);
    Task<AccountResponseModel> GetAccountAsync(int userId, CancellationToken cancellationToken = default);
    Task<AccountBalanceModel> GetBalanceAsync(int userId, CancellationToken cancellationToken = default);
}
