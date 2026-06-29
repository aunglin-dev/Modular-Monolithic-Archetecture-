using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.DbContexts;

public class AppDbContext
    : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(
        Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(
        Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.FullName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .IsRequired();

            entity.Property(x => x.Role)
                .HasMaxLength(30)
                .IsRequired();
        });
    }
}