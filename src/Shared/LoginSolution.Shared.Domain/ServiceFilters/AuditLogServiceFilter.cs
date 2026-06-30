using LoginSolution.Shared.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoginSolution.Shared.Domain.ServiceFilters;

public sealed class AuditLogServiceFilter : IAsyncActionFilter { private readonly IAuditLogService _auditLogService; public AuditLogServiceFilter(IAuditLogService auditLogService) => _auditLogService = auditLogService; public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) { var executed = await next(); var action = $"{context.RouteData.Values["controller"]}.{context.RouteData.Values["action"]}"; await _auditLogService.RecordAsync(action, executed.Exception?.Message, context.HttpContext.RequestAborted); } }
