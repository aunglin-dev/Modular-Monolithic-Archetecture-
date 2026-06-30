using LoginSolution.Shared.Database.Context;
using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Database.Interfaces.Repositories;
using LoginSolution.Shared.Domain.Constants;
using LoginSolution.Shared.Domain.Exceptions;
using LoginSolution.Shared.Domain.Interfaces.Services;
using LoginSolution.Shared.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LoginSolution.Admin.Api.Services;

public sealed class AdminAuthenticationService : Interfaces.IAdminAuthenticationService
{
    private readonly LoginDbContext _db; private readonly IUserRepository _users; private readonly IPasswordHasher<User> _hasher; private readonly ITokenService _tokens;
    public AdminAuthenticationService(LoginDbContext db, IUserRepository users, IPasswordHasher<User> hasher, ITokenService tokens) { _db = db; _users = users; _hasher = hasher; _tokens = tokens; }
    public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model, string ip, string agent, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(model.Email, ct);
        if (user is null) throw new UnauthorizedException(MessageConstants.InvalidCredentials);
        var roles = user.UserRoles.Select(x => x.Role.Name).ToArray();
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        var ok = user.IsActive && roles.Contains(RoleConstants.Admin) && !roles.Contains(RoleConstants.Customer) && result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
        await _db.LoginHistories.AddAsync(new LoginHistory { UserId = user.Id, IpAddress = ip, UserAgent = agent, IsSuccessful = ok, FailureReason = ok ? null : "Invalid credentials or role", LoginDate = DateTime.UtcNow }, ct);
        if (!ok) { await _db.SaveChangesAsync(ct); throw new UnauthorizedException(MessageConstants.InvalidCredentials); }
        if (result == PasswordVerificationResult.SuccessRehashNeeded) user.PasswordHash = _hasher.HashPassword(user, model.Password);
        var response = _tokens.CreateLoginResponse(user.Id, user.Username, user.Email, roles); user.RefreshToken = response.RefreshToken; user.RefreshTokenExpiresAt = response.RefreshTokenExpiresAt; await _db.SaveChangesAsync(ct); return response;
    }
}
