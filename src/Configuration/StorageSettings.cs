namespace tms.Configuration;
public class StorageSettings
{
  public string? SQLiteDbPath { get; set; }
  public string? LocalFileStoragePath { get; set; }
  public string? RemoteDbConnectionString { get; set; }
}
