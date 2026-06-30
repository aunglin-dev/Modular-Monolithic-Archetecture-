namespace LoginSolution.Shared.Database.Entities;

public sealed class LoginHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime LoginDate { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public string? FailureReason { get; set; }
    public User User { get; set; } = null!;
}
