using LoginSolution.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginSolution.Shared.Database.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> b)
    { b.HasKey(x => x.Id); b.Property(x => x.AccountNumber).HasMaxLength(20).IsRequired(); b.Property(x => x.AccountName).HasMaxLength(120).IsRequired(); b.Property(x => x.Balance).HasColumnType("decimal(18,2)"); b.HasIndex(x => x.AccountNumber).IsUnique(); b.HasIndex(x => x.UserId); }
}
