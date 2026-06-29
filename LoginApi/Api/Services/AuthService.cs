using Api.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> ValidateUserAsync(
        string email,
        string password)
    {
        User? user =
            await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        PasswordVerificationResult result =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return user;
    }
}