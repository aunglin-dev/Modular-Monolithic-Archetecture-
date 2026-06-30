using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LoginSolution.Shared.Domain.Constants;
using LoginSolution.Shared.Domain.Interfaces.Services;
using LoginSolution.Shared.Domain.Models.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace LoginSolution.IBanking.Api.Services;

public sealed class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration) => _configuration = configuration;
    public LoginResponseModel CreateLoginResponse(int userId, string username, string email, IReadOnlyList<string> roles)
    {
        var expires = DateTime.UtcNow.AddMinutes(GetInt("JwtSettings:AccessTokenMinutes", 30));
        var key = _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey is missing. Store it in user secrets.");
        var claims = new List<Claim> { new(ClaimConstants.UserId, userId.ToString()), new(ClaimConstants.Username, username), new(ClaimTypes.NameIdentifier, userId.ToString()), new(ClaimTypes.Name, username), new(ClaimTypes.Email, email), new("is_active", "true") };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"], claims, expires: expires, signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
        var refreshExpires = DateTime.UtcNow.AddDays(GetInt("JwtSettings:RefreshTokenDays", 7));
        return new LoginResponseModel { IsSuccess = true, Message = "Login successful.", AccessToken = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = GenerateRefreshToken(), AccessTokenExpiresAt = expires, RefreshTokenExpiresAt = refreshExpires, UserId = userId, Username = username, Email = email, Roles = roles };
    }
    public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private int GetInt(string key, int fallback) => int.TryParse(_configuration[key], out var value) ? value : fallback;
}
