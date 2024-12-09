using tms.Services;
using tms.Data.Context;
using tms.Utils;
using tms.Services.Storage;
using Microsoft.EntityFrameworkCore;
using tms.Configuration;
using Microsoft.Extensions.Options;
using tms.Services.Printer;
using System.Text.Json;
using SixLabors.ImageSharp;

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
builder.Services.AddScoped<StorageConfigurationService>();
builder.Services.AddScoped<PrinterConfigurationService>();


builder.Services.AddScoped<RevenueService>();
builder.Services.AddScoped<FiscalYearService>();
builder.Services.AddScoped<DateConversionService>();

// Register SQLite DbContext for local database with EF Core
builder.Services.AddDbContext<LocalDbContext>((serviceProvider, options) =>
{
   var storageSettings = serviceProvider.GetRequiredService<IOptions<StorageSettings>>().Value;
   string connString = $"Data Source={storageSettings.SQLiteDbPath};";
  //string connString = "Data Source=app.db";
  Console.WriteLine("------------------------------------");
  Console.WriteLine(connString);
  options.UseSqlite(connString);
  // options.UseSqlite($"Data Source=tmssqlite.db;");
});

// builder.Services.AddDbContext<RemoteDbContext>((serviceProvider, options) =>
// {
//   var storageSettings = serviceProvider.GetRequiredService<IOptions<StorageSettings>>().Value;
//   options.UseMySql(
//           storageSettings.RemoteDbConnectionString,
//           ServerVersion.AutoDetect(storageSettings.RemoteDbConnectionString),
//           mySqlOptions =>
//           {
//             mySqlOptions.EnableRetryOnFailure(
//                 maxRetryCount: 20,
//                 maxRetryDelay: TimeSpan.FromSeconds(10),
//                 errorNumbersToAdd: null
//             );
//           }
//         );
// });
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
builder.Services.AddScoped<IPrinterService, PrinterService>();
builder.Services.AddScoped<SQLiteStorageService>();
builder.Services.AddScoped<OnlineDbStorageService>();
builder.Services.AddScoped<FileStorageService>(serviceProvider =>
{
  var storageSettings = serviceProvider.GetRequiredService<IOptions<StorageSettings>>().Value;
  return new FileStorageService(storageSettings.LocalFileStoragePath);
});

// Register the StorageFactory after registering individual storage services
builder.Services.AddScoped<StorageFactory>();

//Ticket 
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<PriceService>();

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
app.MapRazorComponents<tms.Components.App>()
    .AddInteractiveServerRenderMode();


var allowedIps = builder.Configuration.GetSection("AllowedIps").Get<string[]>();
app.Use(async (context, next) =>
{
    var clientIp = context.Connection.RemoteIpAddress?.ToString();
    Console.WriteLine($"INCOMING REQUEST FROM : {clientIp}");
    if (clientIp != null && allowedIps.Contains(clientIp))
    {
        await next();
    }
    else
    {
        context.Response.StatusCode = 403; // Forbidden
        await context.Response.WriteAsync("Access Denied");
    }
});

app.Run();

// BrowserHelper.OpenBrowser("https://localhost:5000/ticket");
