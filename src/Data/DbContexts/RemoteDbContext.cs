using Microsoft.EntityFrameworkCore;
using tms.Data;

public class RemoteDbContext : DbContext
{
  public RemoteDbContext(DbContextOptions options) : base(options) { }
  public DbSet<Ticket> Tickets { get; set; } = null!;
}
