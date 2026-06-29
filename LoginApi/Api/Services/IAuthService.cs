using Domain.Entities;

namespace Api.Services;

public interface IAuthService
{
    Task<User?> ValidateUserAsync(
        string email,
        string password);
}