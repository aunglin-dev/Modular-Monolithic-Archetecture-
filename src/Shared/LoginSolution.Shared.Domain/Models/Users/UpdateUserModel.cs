using System.ComponentModel.DataAnnotations;

namespace LoginSolution.Shared.Domain.Models.Users;

public sealed class UpdateUserModel
{
    [Range(1, int.MaxValue)] public int Id { get; set; }
    [Required, StringLength(80)] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = string.Empty;
    [Required, StringLength(80)] public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(80)] public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    [Required, StringLength(50)] public string RoleName { get; set; } = string.Empty;
}
