namespace Api.Models;

public record LoginResponse(
    string Token,
    string FullName,
    string Email,
    string Role);