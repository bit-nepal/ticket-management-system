using tms.Services.Interfaces;
namespace tms.Services.Storage;

public class SQLiteStorageService : IStorageService
{
  // private readonly AppDbContext _context;
  //
  // public SQLiteStorageService(AppDbContext context)
  // {
  //   _context = context;
  // }
  //
  // public async Task AddRecordAsync(DataModel record)
  // {
  //   await _context.Records.AddAsync(record);
  //   await _context.SaveChangesAsync();
  // }
  //
  // public async Task<List<DataModel>> GetAllRecordsAsync()
  // {
  //   return await _context.Records.ToListAsync();
  // }
}
