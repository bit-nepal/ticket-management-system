using Microsoft.EntityFrameworkCore;
using tms.Data;
namespace tms.Data.Context;
public class LocalDbContext : DbContext
{
  public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }
  public DbSet<Ticket> Tickets { get; set; } = null!;
}
