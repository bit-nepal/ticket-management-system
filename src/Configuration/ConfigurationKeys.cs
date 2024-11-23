namespace tms.Configuration;

public static class ConfigurationKeys
{
  public const string ConfigFileName = "appsettings.json";

  // ------------------ Storage --------------------//
  public const string StorageSettings = "StorageSettings";
  public const string SQLiteDbPath = "StorageSettings:SQLiteDbPath";
  public const string LocalFileStoragePath = "StorageSettings:LocalFileStoragePath";
  public const string RemoteDbConnectionString = "StorageSettings:RemoteDbConnectionString";

  // ------------------ Printer ------------------ //
  public const string PrinterSettings = "PrinterSettings";
  public const string PrinterHost = PrinterSettings + ":" + nameof(PrinterHost);
  public const string PrinterPort = PrinterSettings + ":" + nameof(PrinterPort);
  public const string PrinterComPort = PrinterSettings + ":" + nameof(PrinterComPort);

  //------------------- Ticket ----------------------//
  public const string TicketSettings = "TicketSettings";
}
