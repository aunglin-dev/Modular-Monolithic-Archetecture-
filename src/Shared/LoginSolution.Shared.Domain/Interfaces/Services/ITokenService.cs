using LoginSolution.Shared.Domain.Models.Authentication;

namespace LoginSolution.Shared.Domain.Interfaces.Services;

public interface ITokenService { LoginResponseModel CreateLoginResponse(int userId, string username, string email, IReadOnlyList<string> roles); string GenerateRefreshToken(); }
