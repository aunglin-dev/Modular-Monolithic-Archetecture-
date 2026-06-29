using System.Net.Http.Headers;
using System.Net.Http.Json;
using WBlazor.Models;

namespace WBlazor.Services
{
    public interface IAuthApiService
    {
        Task<(bool Success, string? Error, MessageResponse? Response)> RegisterAsync(RegisterRequest request);

        Task<(bool Success, string? Error, AuthResponse? Response)> LoginAsync(LoginRequest request);

        Task LogoutAsync();
    }

    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthSession _authSession;

        public AuthApiService(HttpClient httpClient, AuthSession authSession)
        {
            _httpClient = httpClient;
            _authSession = authSession;
        }

        public async Task<(bool Success, string? Error, MessageResponse? Response)> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadFromJsonAsync<MessageResponse>();
                return (true, null, body);
            }

            var error = await ReadErrorMessageAsync(response);
            return (false, error, null);
        }

        public async Task<(bool Success, string? Error, AuthResponse? Response)> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (body is not null)
                {
                    _authSession.AccessToken = body.Token;
                    _authSession.ExpiresAt = body.ExpiresAt;
                    _authSession.User = body.User;
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", body.Token);
                }

                return (true, null, body);
            }

            var error = await ReadErrorMessageAsync(response);
            return (false, error, null);
        }

        public async Task LogoutAsync()
        {
            if (_authSession.IsAuthenticated)
            {
                try
                {
                    await _httpClient.PostAsync("api/auth/logout", null);
                }
                catch
                {
                    // Client-side logout still clears local session.
                }
            }

            _authSession.AccessToken = null;
            _authSession.ExpiresAt = null;
            _authSession.User = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
        {
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                if (problem is not null && problem.TryGetValue("message", out var message))
                {
                    return message?.ToString() ?? "Request failed.";
                }
            }
            catch
            {
                // Fall through to default message.
            }

            return response.StatusCode switch
            {
                System.Net.HttpStatusCode.Unauthorized => "Invalid email or password.",
                System.Net.HttpStatusCode.BadRequest => "Invalid request.",
                _ => "Request failed."
            };
        }
    }
}
