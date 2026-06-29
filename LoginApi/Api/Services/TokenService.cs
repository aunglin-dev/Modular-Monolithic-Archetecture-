using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        IConfigurationSection jwtSection =
            _configuration.GetSection("Jwt");

        string key =
            jwtSection["Key"]
            ?? throw new InvalidOperationException(
                "JWT key is missing.");

        string issuer =
            jwtSection["Issuer"]
            ?? throw new InvalidOperationException(
                "JWT issuer is missing.");

        string audience =
            jwtSection["Audience"]
            ?? throw new InvalidOperationException(
                "JWT audience is missing.");

        int expiryMinutes = int.Parse(
            jwtSection["ExpiryMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),

            new("name", user.FullName),
            new("email", user.Email),
            new("role", user.Role)
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                expiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}