using Domain.DbContexts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mvc.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        string normalizedEmail = email.Trim().ToLowerInvariant();

        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user =>
                user.Email == normalizedEmail &&
                user.IsActive);
    }
}