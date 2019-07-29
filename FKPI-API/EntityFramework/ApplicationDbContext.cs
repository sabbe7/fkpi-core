using FKPI_API.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Account>(entity =>
        {
            entity.HasOne(x => x.ParentAccount)
            .WithMany(x => x.Children)
            .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Name).HasMaxLength(200);

            entity.Property(x => x.AccountId).ValueGeneratedNever();
        });

        builder.Entity<AccountValue>(entity =>
        {
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");

            entity.Property(x => x.AccountValueId).ValueGeneratedNever();
        });

        builder.Entity<KPI>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(200);
        });
    }

    public DbSet<Account> Account { get; set; }
    public DbSet<AccountValue> AccountValue { get; set; }
    public DbSet<KPI> KPI { get; set; }
    public DbSet<User> User { get; set; }
}