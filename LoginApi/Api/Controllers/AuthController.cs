using System.IdentityModel.Tokens.Jwt;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(
        IAuthService authService,
        ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request)
    {
        var user = await _authService.ValidateUserAsync(
            request.Email,
            request.Password);

        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        string token =
            _tokenService.CreateToken(user);

        var response = new LoginResponse(
            token,
            user.FullName,
            user.Email,
            user.Role);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            id = User.FindFirst(
                JwtRegisteredClaimNames.Sub)?.Value,

            fullName = User.FindFirst("name")?.Value,

            email = User.FindFirst("email")?.Value,

            role = User.FindFirst("role")?.Value
        });
    }
}