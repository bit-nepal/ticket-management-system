using tms.Services.Interfaces;
namespace tms.Services.Storage;

public class FileStorageService : IStorageService
{
  private readonly string _filePath;

  public FileStorageService(string filePath)
  {
    _filePath = filePath;
  }
  //
  // public async Task SaveRecordAsync(DataModel record)
  // {
  //   var records = await LoadRecordsAsync();
  //   records.Add(record);
  //
  //   var json = JsonSerializer.Serialize(records);
  //   await File.WriteAllTextAsync(_filePath, json);
  // }
  //
  // public async Task<List<DataModel>> LoadRecordsAsync()
  // {
  //   if (!File.Exists(_filePath)) return new List<DataModel>();
  //
  //   var json = await File.ReadAllTextAsync(_filePath);
  //   return JsonSerializer.Deserialize<List<DataModel>>(json);
  // }
}
