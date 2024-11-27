using Microsoft.Extensions.Options;
using tms.Configuration;

namespace tms.Services;
public class PrinterConfigurationService
{
  private readonly IConfiguration _configuration;
  private readonly PrinterSettings _printerSettings;

  public PrinterConfigurationService(IConfiguration configuration, IOptions<PrinterSettings> options)
  {
    _configuration = configuration;
    _printerSettings = options.Value;
    Console.WriteLine($"PrinterSettings.Host: {_printerSettings.Host}");
    Console.WriteLine($"PrinterSettings.Port: {_printerSettings.Port}");
  }

  // public string? GetHost() => _printerSettings.Host;
  public string? GetHost() => _configuration[$"{ConfigurationKeys.PrinterSettings}:PrinterHost"];
  public int? GetPort() => int.TryParse(_configuration[$"{ConfigurationKeys.PrinterSettings}:PrinterPort"], out var port) ? port : null;
  public string? GetComPort() => _configuration[$"{ConfigurationKeys.PrinterSettings}:PrinterComPort"];

  public void SetHost(string host) => UpdateConfig(ConfigurationKeys.PrinterHost, host);
  public void SetComPort(string host) => UpdateConfig(ConfigurationKeys.PrinterComPort, host);
  public void SetPort(int port) => UpdateConfig(ConfigurationKeys.PrinterPort, Convert.ToString(port));

  private void UpdateConfig(string key, string value)
  {
    Console.WriteLine($"Updating {ConfigurationKeys.ConfigFileName}   Key : {key}   value : {value}");
    var json = System.IO.File.ReadAllText(ConfigurationKeys.ConfigFileName);
    var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
    if (jsonObj is null) return;
    jsonObj[ConfigurationKeys.PrinterSettings][key.Split(':')[1]] = value;
    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
    System.IO.File.WriteAllText(ConfigurationKeys.ConfigFileName, output);
  }
}
