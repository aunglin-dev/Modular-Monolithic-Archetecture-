using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;

namespace LoginSolution.Shared.Database.Repositories;

public sealed class AuditLogRepository : IAuditLogRepository
{
    private readonly LoginDbContext _db; public AuditLogRepository(LoginDbContext db) => _db = db;
    public async Task AddAsync(AuditLog auditLog, CancellationToken ct = default) => await _db.AuditLogs.AddAsync(auditLog, ct);
    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
