using LoginSolution.Admin.Api.Mappings;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using LoginSolution.Shared.Domain.Exceptions;
using LoginSolution.Shared.Domain.Models.Roles;

namespace LoginSolution.Admin.Api.Services;

public sealed class RoleManagementService : Interfaces.IRoleManagementService
{
    private readonly IRoleRepository _roles; public RoleManagementService(IRoleRepository roles) => _roles = roles;
    public async Task<IReadOnlyList<RoleResponseModel>> GetRolesAsync(CancellationToken ct = default) => (await _roles.GetAllAsync(ct)).Select(x => x.ToResponse()).ToArray();
    public async Task<RoleResponseModel> GetRoleAsync(int id, CancellationToken ct = default) => (await _roles.GetByIdAsync(id, ct) ?? throw new NotFoundException("Role was not found.")).ToResponse();
    public async Task<RoleResponseModel> CreateRoleAsync(CreateRoleModel model, CancellationToken ct = default) { if (await _roles.GetByNameAsync(model.Name, ct) is not null) throw new ValidationException("Role already exists."); var role = new Role { Name = model.Name, Description = model.Description, IsActive = true, CreatedAt = DateTime.UtcNow }; await _roles.AddAsync(role, ct); await _roles.SaveChangesAsync(ct); return role.ToResponse(); }
    public async Task<RoleResponseModel> UpdateRoleAsync(int id, CreateRoleModel model, CancellationToken ct = default) { var role = await _roles.GetByIdAsync(id, ct) ?? throw new NotFoundException("Role was not found."); role.Name = model.Name; role.Description = model.Description; await _roles.SaveChangesAsync(ct); return role.ToResponse(); }
}
