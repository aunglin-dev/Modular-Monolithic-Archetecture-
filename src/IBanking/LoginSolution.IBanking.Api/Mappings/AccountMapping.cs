using LoginSolution.Shared.Database.Entities;
using LoginSolution.Shared.Domain.Models.Accounts;

namespace LoginSolution.IBanking.Api.Mappings;

public static class AccountMapping
{
    public static AccountResponseModel ToResponse(this Account account) => new() { Id = account.Id, AccountNumber = account.AccountNumber, AccountName = account.AccountName, Balance = account.Balance, Status = account.Status.ToString(), CreatedAt = account.CreatedAt };
    public static AccountBalanceModel ToBalance(this Account account) => new() { AccountNumber = account.AccountNumber, Balance = account.Balance, AsOfUtc = DateTime.UtcNow };
}
