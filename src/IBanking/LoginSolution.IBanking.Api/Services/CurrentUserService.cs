using System.Security.Claims;
using LoginSolution.Shared.Domain.Constants;
using LoginSolution.Shared.Domain.Interfaces.Services;

namespace LoginSolution.IBanking.Api.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;
    public CurrentUserService(IHttpContextAccessor http) => _http = http;
    public int? UserId => int.TryParse(Principal.FindFirstValue(ClaimConstants.UserId) ?? Principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
    public string? Email => Principal.FindFirstValue(ClaimTypes.Email);
    public ClaimsPrincipal Principal => _http.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
}
