using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using WBlazor.Models;

namespace WBlazor.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthSession _authSession;

        public JwtAuthenticationStateProvider(AuthSession authSession)
        {
            _authSession = authSession;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!_authSession.IsAuthenticated || _authSession.User is null)
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var user = _authSession.User;
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.Name, user.FullName)
            };

            var identity = new ClaimsIdentity(claims, authenticationType: "jwt");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
