using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WApi.DTOs;
using WApi.Services;

namespace WApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (success, error, response) = await _authService.RegisterAsync(request, cancellationToken);

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return Created(string.Empty, response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (success, error, response) = await _authService.LoginAsync(request, cancellationToken);

            if (!success)
            {
                return Unauthorized(new { message = error });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Logout()
        {
            return Ok(new MessageResponseDto { Message = "Logged out successfully." });
        }
    }
}
