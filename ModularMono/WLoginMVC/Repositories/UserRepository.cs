using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WLoginMVC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user;
        }
    }
}
