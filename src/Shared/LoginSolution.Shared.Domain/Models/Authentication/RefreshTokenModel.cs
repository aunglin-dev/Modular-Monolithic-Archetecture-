using System.ComponentModel.DataAnnotations;

namespace LoginSolution.Shared.Domain.Models.Authentication;

public sealed class RefreshTokenModel
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
