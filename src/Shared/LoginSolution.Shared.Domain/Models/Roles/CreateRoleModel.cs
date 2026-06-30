using System.ComponentModel.DataAnnotations;

namespace LoginSolution.Shared.Domain.Models.Roles;

public sealed class CreateRoleModel
{
    [Required, StringLength(50)] public string Name { get; set; } = string.Empty;
    [StringLength(200)] public string Description { get; set; } = string.Empty;
}
