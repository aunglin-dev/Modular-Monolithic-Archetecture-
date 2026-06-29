using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Blazor.Services;

public class JwtAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";

    private readonly IJSRuntime _jsRuntime;

    private static readonly AuthenticationState AnonymousState =
        new(
            new ClaimsPrincipal(
                new ClaimsIdentity()));

    public JwtAuthenticationStateProvider(
        IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        string? token =
            await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return AnonymousState;
        }

        try
        {
            List<Claim> claims =
                ParseClaimsFromJwt(token);

            if (IsTokenExpired(claims))
            {
                await _jsRuntime.InvokeVoidAsync(
                    "localStorage.removeItem",
                    TokenKey);

                return AnonymousState;
            }

            var identity = new ClaimsIdentity(
                claims,
                authenticationType: "jwt",
                nameType: "name",
                roleType: "role");

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                TokenKey);

            return AnonymousState;
        }
    }

    public async Task MarkUserAsAuthenticatedAsync(
        string token)
    {
        await _jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            TokenKey,
            token);

        List<Claim> claims =
            ParseClaimsFromJwt(token);

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "jwt",
            nameType: "name",
            roleType: "role");

        var user = new ClaimsPrincipal(identity);

        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(
            Task.FromResult(state));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        await _jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            TokenKey);

        NotifyAuthenticationStateChanged(
            Task.FromResult(AnonymousState));
    }

    private static bool IsTokenExpired(
        IEnumerable<Claim> claims)
    {
        Claim? expiryClaim =
            claims.FirstOrDefault(
                claim => claim.Type == "exp");

        if (expiryClaim is null)
        {
            return true;
        }

        if (!long.TryParse(
                expiryClaim.Value,
                out long expirySeconds))
        {
            return true;
        }

        long currentSeconds =
            DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return expirySeconds <= currentSeconds;
    }

    private static List<Claim> ParseClaimsFromJwt(
        string token)
    {
        string[] tokenParts = token.Split('.');

        if (tokenParts.Length < 2)
        {
            throw new InvalidOperationException(
                "Invalid JWT token.");
        }

        byte[] jsonBytes =
            ParseBase64WithoutPadding(tokenParts[1]);

        using JsonDocument document =
            JsonDocument.Parse(jsonBytes);

        var claims = new List<Claim>();

        foreach (JsonProperty property
                 in document.RootElement
                     .EnumerateObject())
        {
            if (property.Value.ValueKind ==
                JsonValueKind.Array)
            {
                foreach (JsonElement item
                         in property.Value
                             .EnumerateArray())
                {
                    claims.Add(
                        new Claim(
                            property.Name,
                            item.ToString()));
                }

                continue;
            }

            claims.Add(
                new Claim(
                    property.Name,
                    property.Value.ToString()));
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(
        string base64)
    {
        string value = base64
            .Replace('-', '+')
            .Replace('_', '/');

        value = (value.Length % 4) switch
        {
            2 => value + "==",
            3 => value + "=",
            _ => value
        };

        return Convert.FromBase64String(value);
    }
}