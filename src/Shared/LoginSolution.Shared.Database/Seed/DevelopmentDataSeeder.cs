using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Domain.Constants;
using LoginSolution.Shared.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginSolution.Shared.Database.Seed;

public sealed class DevelopmentDataSeeder
{
    private readonly LoginDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;
    public DevelopmentDataSeeder(LoginDbContext db, IPasswordHasher<User> passwordHasher) { _db = db; _passwordHasher = passwordHasher; }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var adminRole = await EnsureRoleAsync(RoleConstants.Admin, "Banking administrators", cancellationToken);
        var customerRole = await EnsureRoleAsync(RoleConstants.Customer, "Banking customers", cancellationToken);
        var admin = await EnsureUserAsync("demo.admin", "admin@loginsolution.local", "Demo", "Administrator", "Admin@12345", cancellationToken);
        var customer = await EnsureUserAsync("demo.customer", "customer@loginsolution.local", "Demo", "Customer", "Customer@12345", cancellationToken);
        await EnsureUserRoleAsync(admin.Id, adminRole.Id, cancellationToken);
        await EnsureUserRoleAsync(customer.Id, customerRole.Id, cancellationToken);
        await EnsureAccountAsync(customer.Id, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<Role> EnsureRoleAsync(string name, string description, CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(x => x.Name == name, ct);
        if (role is not null) return role;
        role = new Role { Name = name, Description = description, IsActive = true, CreatedAt = DateTime.UtcNow };
        await _db.Roles.AddAsync(role, ct); await _db.SaveChangesAsync(ct); return role;
    }

    private async Task<User> EnsureUserAsync(string username, string email, string firstName, string lastName, string password, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email || x.Username == username, ct);
        if (user is not null) return user;
        user = new User { Username = username, Email = email, FirstName = firstName, LastName = lastName, IsActive = true, CreatedAt = DateTime.UtcNow };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        await _db.Users.AddAsync(user, ct); await _db.SaveChangesAsync(ct); return user;
    }

    private async Task EnsureUserRoleAsync(int userId, int roleId, CancellationToken ct)
    {
        if (!await _db.UserRoles.AnyAsync(x => x.UserId == userId && x.RoleId == roleId, ct))
            await _db.UserRoles.AddAsync(new Entities.UserRole { UserId = userId, RoleId = roleId }, ct);
    }

    private async Task EnsureAccountAsync(int userId, CancellationToken ct)
    {
        const string accountNumber = "1000000001";
        if (!await _db.Accounts.AnyAsync(x => x.AccountNumber == accountNumber, ct))
            await _db.Accounts.AddAsync(new Account { UserId = userId, AccountNumber = accountNumber, AccountName = "Demo Customer Account", Balance = 100000.00m, Status = AccountStatus.Active, CreatedAt = DateTime.UtcNow }, ct);
    }
}
