namespace WBlazor.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public UserInfo User { get; set; } = new();
    }
}
