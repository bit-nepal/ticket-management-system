using Microsoft.EntityFrameworkCore;
using tms.Data;
namespace tms.Data.Context;
public class LocalDbContext : DbContext
{
  public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }
  public DbSet<Ticket> Tickets { get; set; } = null!;

  public DbSet<DailyRevenue> DailyRevenues { get; set; } = null!;
  public DbSet<MonthlyRevenue> MonthlyRevenues { get; set; } = null!;
  public DbSet<YearlyRevenue> YearlyRevenues { get; set; } = null!;
  public DbSet<RevenueCell> RevenueCells { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<DailyRevenue>(entity =>
       {
         entity.OwnsOne(d => d.DateBS); // Configures NepaliDate as a complex type
       });

        modelBuilder.Entity<Ticket>(entity => entity.OwnsOne(d => d.NepaliDate));
    modelBuilder.Entity<DailyRevenue>()
        .HasOne(dr => dr.MonthlyRevenue)
        .WithMany(mr => mr.DailyRevenues)
        .HasForeignKey(dr => dr.MonthlyRevenueId);

    modelBuilder.Entity<MonthlyRevenue>()
        .HasOne(mr => mr.YearlyRevenue)
        .WithMany(yr => yr.MonthlyRevenues)
        .HasForeignKey(mr => mr.YearlyRevenueId);

    modelBuilder.Entity<RevenueCell>()
        .HasOne(rc => rc.DailyRevenue)
        .WithMany(dr => dr.RevenueCells)
        .HasForeignKey(rc => rc.DailyRevenueId);
  }
}
