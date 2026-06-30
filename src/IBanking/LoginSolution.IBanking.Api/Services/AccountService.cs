using LoginSolution.IBanking.Api.Mappings;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using LoginSolution.Shared.Domain.Exceptions;
using LoginSolution.Shared.Domain.Models.Accounts;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.IBanking.Api.Services;

public sealed class AccountService : Interfaces.IAccountService
{
    private readonly IUserRepository _users; private readonly IAccountRepository _accounts;
    public AccountService(IUserRepository users, IAccountRepository accounts) { _users = users; _accounts = accounts; }
    public async Task<UserResponseModel> GetProfileAsync(int userId, CancellationToken ct = default) => (await _users.GetByIdAsync(userId, ct) ?? throw new NotFoundException("Profile was not found.")).ToResponse();
    public async Task<AccountResponseModel> GetAccountAsync(int userId, CancellationToken ct = default) => (await _accounts.GetByUserIdAsync(userId, ct) ?? throw new NotFoundException("Account was not found.")).ToResponse();
    public async Task<AccountBalanceModel> GetBalanceAsync(int userId, CancellationToken ct = default) => (await _accounts.GetByUserIdAsync(userId, ct) ?? throw new NotFoundException("Account was not found.")).ToBalance();
}
