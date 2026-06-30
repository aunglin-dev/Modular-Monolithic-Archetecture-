using LoginSolution.Shared.Database.Entities;

namespace LoginSolution.Shared.Database.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Account account, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
