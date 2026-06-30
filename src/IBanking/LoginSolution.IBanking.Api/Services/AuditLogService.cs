using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using LoginSolution.Shared.Domain.Interfaces.Services;

namespace LoginSolution.IBanking.Api.Services;

public sealed class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repo; private readonly ICurrentUserService _current; private readonly IHttpContextAccessor _http;
    public AuditLogService(IAuditLogRepository repo, ICurrentUserService current, IHttpContextAccessor http) { _repo = repo; _current = current; _http = http; }
    public async Task RecordAsync(string action, string? details, CancellationToken cancellationToken = default)
    {
        var ctx = _http.HttpContext;
        await _repo.AddAsync(new AuditLog { UserId = _current.UserId, Action = action, Controller = ctx?.Request.RouteValues["controller"]?.ToString() ?? string.Empty, Method = ctx?.Request.Method ?? string.Empty, RequestPath = ctx?.Request.Path.Value ?? string.Empty, IpAddress = ctx?.Connection.RemoteIpAddress?.ToString() ?? string.Empty, Details = details, CreatedAt = DateTime.UtcNow }, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }
}
