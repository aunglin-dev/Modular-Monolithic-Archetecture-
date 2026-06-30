using LoginSolution.Admin.Api.Mappings;
using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using LoginSolution.Shared.Domain.Exceptions;
using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginSolution.Admin.Api.Services;

public sealed class UserManagementService : Interfaces.IUserManagementService
{
    private readonly LoginDbContext _db; private readonly IUserRepository _users; private readonly IRoleRepository _roles; private readonly IPasswordHasher<User> _hasher;
    public UserManagementService(LoginDbContext db, IUserRepository users, IRoleRepository roles, IPasswordHasher<User> hasher) { _db = db; _users = users; _roles = roles; _hasher = hasher; }
    public async Task<UserListModel> GetUsersAsync(PaginationRequestModel p, CancellationToken ct = default) => new() { Users = (await _users.GetPagedAsync(p.PageNumber, p.PageSize, ct)).Select(x => x.ToResponse()).ToArray(), Pagination = new PaginationResponseModel { PageNumber = p.PageNumber, PageSize = p.PageSize, TotalCount = await _users.CountAsync(ct) } };
    public async Task<UserResponseModel> GetUserAsync(int id, CancellationToken ct = default) => (await _users.GetByIdAsync(id, ct) ?? throw new NotFoundException("User was not found.")).ToResponse();
    public async Task<UserResponseModel> CreateUserAsync(CreateUserModel model, CancellationToken ct = default)
    {
        if (await _users.GetByEmailAsync(model.Email, ct) is not null || await _users.GetByUsernameAsync(model.Username, ct) is not null) throw new ValidationException("Email or username already exists.");
        var role = await _roles.GetByNameAsync(model.RoleName, ct) ?? throw new NotFoundException("Role was not found.");
        var user = UserMapping.ToEntity(model); user.PasswordHash = _hasher.HashPassword(user, model.Password); user.UserRoles.Add(new UserRole { User = user, Role = role }); await _users.AddAsync(user, ct); await _users.SaveChangesAsync(ct); return user.ToResponse();
    }
    public async Task<UserResponseModel> UpdateUserAsync(int id, UpdateUserModel model, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(id, ct) ?? throw new NotFoundException("User was not found."); var role = await _roles.GetByNameAsync(model.RoleName, ct) ?? throw new NotFoundException("Role was not found.");
        user.Apply(model); user.UserRoles.Clear(); user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id }); await _users.SaveChangesAsync(ct); return user.ToResponse();
    }
    public async Task SetStatusAsync(int id, bool isActive, CancellationToken ct = default) { var user = await _users.GetByIdAsync(id, ct) ?? throw new NotFoundException("User was not found."); user.IsActive = isActive; user.UpdatedAt = DateTime.UtcNow; await _users.SaveChangesAsync(ct); }
}
