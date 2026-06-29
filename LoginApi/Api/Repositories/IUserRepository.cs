using Domain.Entities;

namespace Api.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}