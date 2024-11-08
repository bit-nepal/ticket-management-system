using tms.Services.Storage;
namespace tms.Services;

public class StorageFactory
{
  private readonly SQLiteStorageService _sqliteService;
  private readonly OnlineDbStorageService _dbService;
  private readonly FileStorageService _fileService;

  public StorageFactory(SQLiteStorageService sqliteService, OnlineDbStorageService dbService, FileStorageService fileService)
  {
    _sqliteService = sqliteService;
    _dbService = dbService;
    _fileService = fileService;
  }

  public SQLiteStorageService GetPrimaryStorage() => _sqliteService;
  public OnlineDbStorageService GetBackupStorage() => _dbService;
  public FileStorageService GetFileStorage() => _fileService;
}
