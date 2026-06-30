using LoginSolution.Shared.Domain.ServiceFilters;
using Microsoft.Extensions.DependencyInjection;

namespace LoginSolution.Shared.Domain.Extensions;

public static class DomainServiceExtensions { public static IServiceCollection AddDomainServiceFilters(this IServiceCollection services) { services.AddScoped<ValidationServiceFilter>(); services.AddScoped<AuthorizationServiceFilter>(); services.AddScoped<ActiveUserServiceFilter>(); services.AddScoped<AuditLogServiceFilter>(); return services; } }
