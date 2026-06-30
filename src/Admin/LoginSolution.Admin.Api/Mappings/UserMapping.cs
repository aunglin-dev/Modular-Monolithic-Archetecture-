using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.Admin.Api.Mappings;

public static class UserMapping
{
    public static User ToEntity(CreateUserModel model) => new() { Username = model.Username, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, IsActive = true, CreatedAt = DateTime.UtcNow };
    public static void Apply(this User user, UpdateUserModel model) { user.Username = model.Username; user.Email = model.Email; user.FirstName = model.FirstName; user.LastName = model.LastName; user.IsActive = model.IsActive; user.UpdatedAt = DateTime.UtcNow; }
    public static UserResponseModel ToResponse(this User user) => new() { Id = user.Id, Username = user.Username, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, IsActive = user.IsActive, CreatedAt = user.CreatedAt, Roles = user.UserRoles.Select(x => x.Role.Name).ToArray() };
}
