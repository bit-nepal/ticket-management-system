using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace tms.Data.Context;
public class LocalDbContextFactory : IDesignTimeDbContextFactory<LocalDbContext>
{
  public LocalDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<LocalDbContext>();
    optionsBuilder.UseSqlite("Data Source=app.db"); // Update this if needed
    Console.WriteLine("LOCAL DB CONTEXT FACTORY ");
    return new LocalDbContext(optionsBuilder.Options);
  }
}
