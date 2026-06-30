using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LoginSolution.Shared.Database.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly LoginDbContext _db; public RoleRepository(LoginDbContext db) => _db = db;
    public Task<Role?> GetByIdAsync(int id, CancellationToken ct = default) => _db.Roles.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Role?> GetByNameAsync(string name, CancellationToken ct = default) => _db.Roles.FirstOrDefaultAsync(x => x.Name == name, ct);
    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken ct = default) => await _db.Roles.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
    public async Task AddAsync(Role role, CancellationToken ct = default) => await _db.Roles.AddAsync(role, ct);
    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
