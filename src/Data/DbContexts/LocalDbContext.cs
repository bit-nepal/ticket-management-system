using Microsoft.EntityFrameworkCore;
public class LocalDbContext : DbContext
{
  public LocalDbContext(DbContextOptions options) : base(options) { }
}
