using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.IBanking.Api.Mappings;

public static class UserMapping
{
    public static UserResponseModel ToResponse(this User user) => new() { Id = user.Id, Username = user.Username, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, IsActive = user.IsActive, CreatedAt = user.CreatedAt, Roles = user.UserRoles.Select(x => x.Role.Name).ToArray() };
}
