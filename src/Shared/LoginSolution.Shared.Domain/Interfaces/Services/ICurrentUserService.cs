using System.Security.Claims;

namespace LoginSolution.Shared.Domain.Interfaces.Services;

public interface ICurrentUserService { int? UserId { get; } string? Email { get; } ClaimsPrincipal Principal { get; } }
