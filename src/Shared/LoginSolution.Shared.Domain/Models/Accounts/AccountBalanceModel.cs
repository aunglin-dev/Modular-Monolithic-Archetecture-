namespace LoginSolution.Shared.Domain.Models.Accounts;

public sealed class AccountBalanceModel
{
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime AsOfUtc { get; set; } = DateTime.UtcNow;
}
