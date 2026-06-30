using LoginSolution.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginSolution.Shared.Database.Configurations;

public sealed class LoginHistoryConfiguration : IEntityTypeConfiguration<LoginHistory>
{
    public void Configure(EntityTypeBuilder<LoginHistory> b)
    { b.HasKey(x => x.Id); b.Property(x => x.IpAddress).HasMaxLength(64); b.Property(x => x.UserAgent).HasMaxLength(300); b.Property(x => x.FailureReason).HasMaxLength(200); b.HasIndex(x => new { x.UserId, x.LoginDate }); }
}
