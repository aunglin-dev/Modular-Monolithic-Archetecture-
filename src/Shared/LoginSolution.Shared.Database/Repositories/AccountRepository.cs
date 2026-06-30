using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LoginSolution.Shared.Database.Repositories;

public sealed class AccountRepository : IAccountRepository
{
    private readonly LoginDbContext _db; public AccountRepository(LoginDbContext db) => _db = db;
    public Task<Account?> GetByUserIdAsync(int userId, CancellationToken ct = default) => _db.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, ct);
    public Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default) => _db.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber, ct);
    public async Task AddAsync(Account account, CancellationToken ct = default) => await _db.Accounts.AddAsync(account, ct);
    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
