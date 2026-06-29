using Domain.Entities;

namespace Mvc.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}