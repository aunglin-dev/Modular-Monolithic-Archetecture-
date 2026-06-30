using LoginSolution.Shared.Domain.Models.Roles;

namespace LoginSolution.Admin.Api.Services.Interfaces;

public interface IRoleManagementService
{
    Task<IReadOnlyList<RoleResponseModel>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<RoleResponseModel> GetRoleAsync(int id, CancellationToken cancellationToken = default);
    Task<RoleResponseModel> CreateRoleAsync(CreateRoleModel model, CancellationToken cancellationToken = default);
    Task<RoleResponseModel> UpdateRoleAsync(int id, CreateRoleModel model, CancellationToken cancellationToken = default);
}
