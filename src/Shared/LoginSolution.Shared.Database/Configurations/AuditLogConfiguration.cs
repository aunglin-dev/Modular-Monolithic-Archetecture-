using LoginSolution.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginSolution.Shared.Database.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> b)
    { b.HasKey(x => x.Id); b.Property(x => x.Action).HasMaxLength(120).IsRequired(); b.Property(x => x.Controller).HasMaxLength(80); b.Property(x => x.Method).HasMaxLength(10); b.Property(x => x.RequestPath).HasMaxLength(300); b.Property(x => x.IpAddress).HasMaxLength(64); b.HasIndex(x => x.CreatedAt); }
}
