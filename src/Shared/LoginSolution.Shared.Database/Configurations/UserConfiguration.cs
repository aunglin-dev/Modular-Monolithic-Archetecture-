using LoginSolution.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginSolution.Shared.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.HasKey(x => x.Id); b.Property(x => x.Username).HasMaxLength(80).IsRequired(); b.Property(x => x.Email).HasMaxLength(150).IsRequired();
        b.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired(); b.Property(x => x.FirstName).HasMaxLength(80).IsRequired(); b.Property(x => x.LastName).HasMaxLength(80).IsRequired(); b.Property(x => x.RefreshToken).HasMaxLength(200);
        b.HasIndex(x => x.Username).IsUnique(); b.HasIndex(x => x.Email).IsUnique();
        b.HasMany(x => x.UserRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Accounts).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(x => x.LoginHistories).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
