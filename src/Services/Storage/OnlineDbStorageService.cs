using tms.Services.Interfaces;
namespace tms.Services.Storage;

public class OnlineDbStorageService : IStorageService
{
  // private readonly RemoteDbContext _context;
  //
  // public OnlineDbStorageService(RemoteDbContext context)
  // {
  //     _context = context;
  // }
  //
  // public async Task AddRecordAsync(DataModel record)
  // {
  //     await _context.Records.AddAsync(record);
  //     await _context.SaveChangesAsync();
  // }
  //
  // public async Task<List<DataModel>> GetAllRecordsAsync()
  // {
  //     return await _context.Records.ToListAsync();
  // }
}
