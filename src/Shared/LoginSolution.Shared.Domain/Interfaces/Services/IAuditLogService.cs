namespace LoginSolution.Shared.Domain.Interfaces.Services;

public interface IAuditLogService { Task RecordAsync(string action, string? details, CancellationToken cancellationToken = default); }
