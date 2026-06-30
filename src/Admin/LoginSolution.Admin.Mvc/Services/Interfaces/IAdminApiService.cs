using LoginSolution.Shared.Domain.Models.Authentication;
using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Roles;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.Admin.Mvc.Services.Interfaces;

public interface IAdminApiService
{
    Task<LoginResponseModel?> LoginAsync(LoginRequestModel model, CancellationToken cancellationToken = default);
    Task<UserListModel?> GetUsersAsync(int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<UserResponseModel?> GetUserAsync(int id, CancellationToken cancellationToken = default);
    Task<UserResponseModel?> CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken = default);
    Task<UserResponseModel?> UpdateUserAsync(int id, UpdateUserModel model, CancellationToken cancellationToken = default);
    Task<bool> SetUserStatusAsync(int id, bool isActive, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RoleResponseModel>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<RoleResponseModel?> GetRoleAsync(int id, CancellationToken cancellationToken = default);
    Task<RoleResponseModel?> CreateRoleAsync(CreateRoleModel model, CancellationToken cancellationToken = default);
    Task<RoleResponseModel?> UpdateRoleAsync(int id, CreateRoleModel model, CancellationToken cancellationToken = default);
}
