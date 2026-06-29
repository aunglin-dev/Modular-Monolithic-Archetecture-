using AutoMapper;
using Domain.Entities;
using WApi.DTOs;
using WApi.Repositories;

namespace WApi.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Error, MessageResponseDto? Response)> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken cancellationToken = default);

        Task<(bool Success, string? Error, AuthResponseDto? Response)> LoginAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken = default);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        public async Task<(bool Success, string? Error, MessageResponseDto? Response)> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (await _userRepository.EmailExistsAsync(email, cancellationToken))
            {
                return (false, "An account with this email already exists.", null);
            }

            var user = _mapper.Map<User>(request);
            user.Email = email;
            user.PasswordHash = _passwordHasher.Hash(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            await _userRepository.CreateAsync(user, cancellationToken);

            return (true, null, new MessageResponseDto
            {
                Message = "Registration successful. Please log in."
            });
        }

        public async Task<(bool Success, string? Error, AuthResponseDto? Response)> LoginAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null || !user.IsActive || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return (false, "Invalid email or password.", null);
            }

            var (token, expiresAt) = _jwtTokenService.CreateToken(user);

            return (true, null, new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = _mapper.Map<UserResponseDto>(user)
            });
        }
    }
}
