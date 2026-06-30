using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Domain.Models.Roles;

namespace LoginSolution.Admin.Api.Mappings;

public static class RoleMapping
{
    public static RoleResponseModel ToResponse(this Role role) => new() { Id = role.Id, Name = role.Name, Description = role.Description, IsActive = role.IsActive, CreatedAt = role.CreatedAt };
}
