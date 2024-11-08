using Microsoft.Extensions.Options;
using tms.Configuration;

namespace tms.Services;
public class StorageConfigurationService
{
  private readonly IConfiguration _configuration;
  private readonly StorageSettings _storageSettings;

  public StorageConfigurationService(IConfiguration configuration, IOptions<StorageSettings> options)
  {
    _configuration = configuration;
    _storageSettings = options.Value;
  }

  public string? GetSQLiteDbPath() => _storageSettings.SQLiteDbPath;
  public string? GetRemoteDbConnectionString() => _storageSettings.RemoteDbConnectionString;
  public string? GetLocalFileStoragePath() => _storageSettings.LocalFileStoragePath;

  public void SetSQLiteDbPath(string path) => UpdateConfig(ConfigurationKeys.SQLiteDbPath, path);
  public void SetRemoteDbConnectionString(string connectionString) => UpdateConfig(ConfigurationKeys.RemoteDbConnectionString, connectionString);
  public void SetLocalFileStoragePath(string path) => UpdateConfig(ConfigurationKeys.LocalFileStoragePath, path);

  private void UpdateConfig(string key, string value)
  {
    var json = System.IO.File.ReadAllText(ConfigurationKeys.ConfigFileName);
    var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
    jsonObj["StorageSettings"][key.Split(':')[1]] = value;
    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
    System.IO.File.WriteAllText(ConfigurationKeys.ConfigFileName, output);
  }
}
