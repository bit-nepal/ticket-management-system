using tms.Services.Storage;
namespace tms.Services;

public class StorageSyncService
{
  private readonly SQLiteStorageService _localStorage;
  private readonly OnlineDbStorageService _centralStorage;
  private readonly FileStorageService _fileStorage;

  public StorageSyncService(SQLiteStorageService localStorage, OnlineDbStorageService centralStorage, FileStorageService fileStorage)
  {
    _localStorage = localStorage;
    _centralStorage = centralStorage;
    _fileStorage = fileStorage;
  }

  public async Task SyncDataAsync()
  {
    // var unsyncedRecords = await _localStorage.GetUnsyncedRecordsAsync();
    //
    // foreach (var record in unsyncedRecords)
    // {
    //     await _centralStorage.AddRecordAsync(record);
    //     await _fileStorage.SaveRecordAsync(record);
    //     await _localStorage.MarkAsSyncedAsync(record.Id); // Mark as synced
    // }
  }
}
