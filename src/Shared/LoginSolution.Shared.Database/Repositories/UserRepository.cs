using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LoginSolution.Shared.Database.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly LoginDbContext _db; public UserRepository(LoginDbContext db) => _db = db;
    public Task<User?> GetByIdAsync(int id, CancellationToken ct = default) => _db.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) => _db.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Email == email, ct);
    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) => _db.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Username == username, ct);
    public async Task<IReadOnlyList<User>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default) => await _db.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role).OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    public Task<int> CountAsync(CancellationToken ct = default) => _db.Users.AsNoTracking().CountAsync(ct);
    public async Task AddAsync(User user, CancellationToken ct = default) => await _db.Users.AddAsync(user, ct);
    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
