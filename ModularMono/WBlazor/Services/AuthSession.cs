using WBlazor.Models;

namespace WBlazor.Services
{
    public class AuthSession
    {
        public string? AccessToken { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public UserInfo? User { get; set; }

        public bool IsAuthenticated =>
            !string.IsNullOrWhiteSpace(AccessToken)
            && ExpiresAt.HasValue
            && ExpiresAt.Value > DateTime.UtcNow;
    }
}
