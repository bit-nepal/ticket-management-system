using tms.Services;
using tms.Utils;
using tms.Components;
using tms.Services.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using tms.Configuration;
using Microsoft.Extensions.Options;
using tms.Services.Printer;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
// Load configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddBlazorBootstrap();

// bind StorageSettings with the json Section
builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection(ConfigurationKeys.StorageSettings));
// bind PrinterSettings with the json Section
builder.Services.Configure<PrinterSettings>(
    builder.Configuration.GetSection(ConfigurationKeys.PrinterSettings));

// Register Razor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register the storage configuration service
builder.Services.AddSingleton<StorageConfigurationService>();
builder.Services.AddSingleton<PrinterConfigurationService>();

// Register SQLite DbContext for local database with EF Core
builder.Services.AddDbContext<LocalDbContext>((serviceProvider, options) =>
{
  var storageSettings = serviceProvider.GetRequiredService<IOptions<StorageSettings>>().Value;
  options.UseSqlite($"Data Source={storageSettings.SQLiteDbPath}");
});
builder.Services.AddDbContext<RemoteDbContext>((serviceProvider, options) =>
{
  var storageSettings = serviceProvider.GetRequiredService<IOptions<StorageSettings>>().Value;
  options.UseMySql(
          storageSettings.RemoteDbConnectionString,
          ServerVersion.AutoDetect(storageSettings.RemoteDbConnectionString),
          mySqlOptions =>
          {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 20,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
          }
        );
}
    );
// builder.Services.AddDbContextFactory<RemoteDbContext>(
//       (serviceProvider, options) =>
//       {
//         var storageSettings = serviceProvider.GetRequiredService<IOptions<StorageSettings>>().Value;
//         options.UseMySql(
//             storageSettings.RemoteDbConnectionString,
//             ServerVersion.AutoDetect(storageSettings.RemoteDbConnectionString),
//             mySqlOptions =>
//             {
//               mySqlOptions.EnableRetryOnFailure(
//                   maxRetryCount: 20,
//                   maxRetryDelay: TimeSpan.FromSeconds(10),
//                   errorNumbersToAdd: null
//               );
//             }
//           );
//       }
//     );

// Register storage services with necessary configuration parameters
builder.Services.AddSingleton<IPrinterService, PrinterService>();
builder.Services.AddSingleton<SQLiteStorageService>();
builder.Services.AddSingleton<OnlineDbStorageService>();
builder.Services.AddSingleton<FileStorageService>(serviceProvider =>
{
  var storageSettings = serviceProvider.GetRequiredService<IOptions<StorageSettings>>().Value;
  return new FileStorageService(storageSettings.LocalFileStoragePath);
});

// Register the StorageFactory after registering individual storage services
builder.Services.AddSingleton<StorageFactory>();

//Ticket 
builder.Services.AddSingleton<TicketService>();
builder.Services.AddOptions<TicketPricingConfig>()
    .Configure<IConfiguration>((settings, configuration) =>
    {
      var options = new JsonSerializerOptions();
      options.Converters.Add(new EnumDictionaryConverter<Nationality, NationalityPricing>());
      options.Converters.Add(new EnumDictionaryConverter<PersonType, int>());
      options.Converters.Add(new EnumDictionaryConverter<AddOnType, int>());
      configuration.GetSection(ConfigurationKeys.TicketSettings).Bind(settings);
    });

// Build and configure the web application
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
// Map Razor components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// BrowserHelper.OpenBrowser("https://localhost:5000");
