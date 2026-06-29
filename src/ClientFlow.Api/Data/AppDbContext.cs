using ClientFlow.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientFlow.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.CompanyName)
                .HasMaxLength(150);

            entity.Property(c => c.PhoneNumber)
                .HasMaxLength(50);

            entity.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasMany(c => c.Projects)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Description)
                .HasMaxLength(1000);

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(p => p.Budget)
                .HasColumnType("decimal(18,2)");
        });
    }
}