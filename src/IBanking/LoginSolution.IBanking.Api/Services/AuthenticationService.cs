using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using LoginSolution.Shared.Domain.Constants;
using LoginSolution.Shared.Domain.Enums;
using LoginSolution.Shared.Domain.Exceptions;
using LoginSolution.Shared.Domain.Interfaces.Services;
using LoginSolution.Shared.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginSolution.IBanking.Api.Services;

public sealed class AuthenticationService : Interfaces.IAuthenticationService
{
    private readonly LoginDbContext _db; private readonly IUserRepository _users; private readonly IPasswordHasher<User> _hasher; private readonly ITokenService _tokens;
    public AuthenticationService(LoginDbContext db, IUserRepository users, IPasswordHasher<User> hasher, ITokenService tokens) { _db = db; _users = users; _hasher = hasher; _tokens = tokens; }
    public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model, string ip, string agent, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(model.Email, ct);
        if (user is null) throw new UnauthorizedException(MessageConstants.InvalidCredentials);
        var roles = user.UserRoles.Select(x => x.Role.Name).ToArray();
        var ok = user.IsActive && roles.Contains(RoleConstants.Customer) && !roles.Contains(RoleConstants.Admin) && user.Accounts.Any(a => a.Status == AccountStatus.Active);
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        ok = ok && (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded);
        await RecordLoginAsync(user.Id, ip, agent, ok, ok ? null : "Invalid credentials or role", ct);
        if (!ok) throw new UnauthorizedException(MessageConstants.InvalidCredentials);
        if (result == PasswordVerificationResult.SuccessRehashNeeded) user.PasswordHash = _hasher.HashPassword(user, model.Password);
        var response = _tokens.CreateLoginResponse(user.Id, user.Username, user.Email, roles);
        user.RefreshToken = response.RefreshToken; user.RefreshTokenExpiresAt = response.RefreshTokenExpiresAt; await _users.SaveChangesAsync(ct); return response;
    }
    public async Task<LoginResponseModel> RefreshTokenAsync(RefreshTokenModel model, CancellationToken ct = default)
    {
        var user = await _db.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.RefreshToken == model.RefreshToken, ct);
        if (user is null || user.RefreshTokenExpiresAt <= DateTime.UtcNow || !user.IsActive) throw new UnauthorizedException("Refresh token is invalid or expired.");
        var roles = user.UserRoles.Select(x => x.Role.Name).ToArray(); var response = _tokens.CreateLoginResponse(user.Id, user.Username, user.Email, roles);
        user.RefreshToken = response.RefreshToken; user.RefreshTokenExpiresAt = response.RefreshTokenExpiresAt; await _db.SaveChangesAsync(ct); return response;
    }
    public async Task ChangePasswordAsync(int userId, ChangePasswordModel model, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct) ?? throw new NotFoundException("User was not found.");
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);
        if (result is not (PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded)) throw new UnauthorizedException("Current password is incorrect.");
        user.PasswordHash = _hasher.HashPassword(user, model.NewPassword); user.UpdatedAt = DateTime.UtcNow; await _users.SaveChangesAsync(ct);
    }
    private async Task RecordLoginAsync(int userId, string ip, string agent, bool success, string? failure, CancellationToken ct)
    { await _db.LoginHistories.AddAsync(new LoginHistory { UserId = userId, IpAddress = ip, UserAgent = agent, IsSuccessful = success, FailureReason = failure, LoginDate = DateTime.UtcNow }, ct); await _db.SaveChangesAsync(ct); }
}
