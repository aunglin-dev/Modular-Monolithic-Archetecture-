using Domain.Entities;

namespace WLoginMVC.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    }
}
