namespace tms.Configuration;

public static class ConfigurationKeys
{
  public const string ConfigFileName = "appsettings.json";
  public const string StorageSettings = "StorageSettings";
  public const string SQLiteDbPath = "StorageSettings:SQLiteDbPath";
  public const string LocalFileStoragePath = "StorageSettings:LocalFileStoragePath";
  public const string RemoteDbConnectionString = "StorageSettings:RemoteDbConnectionString";
}
