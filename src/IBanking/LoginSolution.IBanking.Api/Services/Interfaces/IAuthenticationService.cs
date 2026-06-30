using LoginSolution.Shared.Domain.Models.Authentication;

namespace LoginSolution.IBanking.Api.Services.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponseModel> LoginAsync(LoginRequestModel model, string ipAddress, string userAgent, CancellationToken cancellationToken = default);
    Task<LoginResponseModel> RefreshTokenAsync(RefreshTokenModel model, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(int userId, ChangePasswordModel model, CancellationToken cancellationToken = default);
}
