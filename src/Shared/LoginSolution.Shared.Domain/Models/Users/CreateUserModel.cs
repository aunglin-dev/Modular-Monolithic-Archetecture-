using System.ComponentModel.DataAnnotations;

namespace LoginSolution.Shared.Domain.Models.Users;

public sealed class CreateUserModel
{
    [Required, StringLength(80)] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = string.Empty;
    [Required, StringLength(80)] public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(80)] public string LastName { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    [Required, Compare(nameof(Password)), DataType(DataType.Password)] public string ConfirmPassword { get; set; } = string.Empty;
    [Required, StringLength(50)] public string RoleName { get; set; } = string.Empty;
}
