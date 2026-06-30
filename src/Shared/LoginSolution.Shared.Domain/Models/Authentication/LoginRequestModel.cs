using System.ComponentModel.DataAnnotations;

namespace LoginSolution.Shared.Domain.Models.Authentication;

public sealed class LoginRequestModel
{
    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
