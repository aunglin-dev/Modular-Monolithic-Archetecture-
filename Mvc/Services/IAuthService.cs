using Domain.Entities;

namespace Mvc.Services;

public interface IAuthService
{
    Task<User?> ValidateUserAsync(string email, string password);
}