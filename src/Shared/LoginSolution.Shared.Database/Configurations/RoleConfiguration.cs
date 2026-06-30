using LoginSolution.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginSolution.Shared.Database.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> b)
    { b.HasKey(x => x.Id); b.Property(x => x.Name).HasMaxLength(50).IsRequired(); b.Property(x => x.Description).HasMaxLength(200); b.HasIndex(x => x.Name).IsUnique(); }
}
