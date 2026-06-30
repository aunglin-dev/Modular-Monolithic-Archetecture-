using LoginSolution.Shared.Domain.Models.Authentication;

namespace LoginSolution.Admin.Api.Services.Interfaces;

public interface IAdminAuthenticationService { Task<LoginResponseModel> LoginAsync(LoginRequestModel model, string ipAddress, string userAgent, CancellationToken cancellationToken = default); }
