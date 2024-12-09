using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using tms.Configuration;
using tms.Data;
using tms.Data.Context;
using tms.Services.Printer;

public class TicketService
{
  private TicketPricingConfig _pricingConfig;
  private IPrinterService _printerService;
  private LocalDbContext _localDbContext;
  private RevenueService _revenueService;
  private DateConversionService _dateConversionService;
  public TicketService(
      IOptions<TicketPricingConfig> pricingConfig,
      IPrinterService printerService,
      RevenueService revenueService,
      LocalDbContext dbContext,
      DateConversionService dateConversionService)
  {
    _pricingConfig = pricingConfig.Value;
    _printerService = printerService;
    _localDbContext = dbContext;
    _revenueService = revenueService;
    _dateConversionService = dateConversionService;
  }

  public int GetBaseTicketPrice(Nationality nationality, PersonType personType)
  {
    return _pricingConfig.Nationalities[nationality].BasePrice[personType];
  }
  public int GetAddOnFee(Nationality nationality, AddOnType addOn)
  {
    var addOns = _pricingConfig.Nationalities[nationality].AddOns;
    return addOns.TryGetValue(addOn, out int fee) ? fee : 0;
  }
  public int CalculateTotalPrice(Ticket ticket)
  {
    var basePrice = GetBaseTicketPrice(ticket.Nationality, ticket.PersonType);
    var addOnFees = ticket.AddOns
        .Select(addOn => GetAddOnFee(ticket.Nationality, addOn.AddOnType) * addOn.Quantity)
        .Sum();
    return ticket.IsGroupVisit
        ? (basePrice * ticket.NoOfPeople) + addOnFees
        : basePrice + addOnFees;
  }

 public async Task<IEnumerable<tms.Data.Ticket>> GetTicketsAsync()
    {
        //var _localDbContext = _localDbContextFactory.CreateDbContext();
        return await _localDbContext.Tickets.ToListAsync();
    }
  public async Task<bool> FinalizeTicket(Ticket ticket)
  {
     //await using  var _localDbContext = _localDbContextFactory.CreateDbContext();

    using var transaction = _localDbContext.Database.BeginTransaction();
    try
    {
      ticket.TotalPrice = CalculateTotalPrice(ticket);
      ticket.BarCodeData = GenerateTicketCode();
      ticket.NepaliDate = _dateConversionService.ConvertEnglishDateToNepaliDate(ticket.TimeStamp);
      foreach (var addOn in ticket.AddOns)
      {
        addOn.TotalPrice = addOn.Quantity * GetAddOnFee(ticket.Nationality, addOn.AddOnType);
      }

      if (!await CreateTicket(ticket)) {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                Console.WriteLine("ERROR Adding Ticket: Rolled BACKed Changes !!");
                return false;
      }

      ticket.TicketNo = ticket.Id;

      if (!await _revenueService.AddTicketSaleAsync(ticket)) { 
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                Console.WriteLine("ERROR Adding Revenue: Rolled BACKed Changes !!");
                return false;
       }
      if (!await _printerService.PrintTicket(ticket)) {
                //await transaction.RollbackAsync();
                transaction.Commit();
                await transaction.DisposeAsync();
                Console.WriteLine("ERROR Printing Ticket: Rolled BACKed Changes !!");
                return false;
        }

      transaction.Commit();
      return true;
    }
    catch (Exception e)
    {
      transaction.Rollback();
      transaction.Dispose();
      return false;
    }
  }
  public async Task<bool> CreateTicket(Ticket ticket)
  {
        Console.Write("Adding Ticket to Database: ");
        //await using var _localDbContext = _localDbContextFactory.CreateDbContext();
        _localDbContext.Add(ticket);
        var result =  await _localDbContext.SaveChangesAsync() > 0;
        Console.WriteLine("Ticket NO: " + ticket.Id);
        return result;
  }
  public TicketPricingConfig GetPricingConfig()
  {
    return _pricingConfig;
  }

  public async Task<Ticket?> VerifyTicket(string BarCode)
  {
        //var _localDbContext = _localDbContextFactory.CreateDbContext();

        return _localDbContext.Tickets
        .Where(x => x.BarCodeData == BarCode)
        .Where(x => x.TimeStamp.Day == DateTime.Now.Day)
        .FirstOrDefault();
  }
  public void UpdatePricingConfig(TicketPricingConfig updatedConfig)
  {
    _pricingConfig = updatedConfig;
  }
  public void UpdatePricingConfig2(TicketPricingConfig updatedConfig)
  {
    _pricingConfig = updatedConfig;
    _pricingConfig.Nationalities = updatedConfig.Nationalities
        .ToDictionary(
            kv => kv.Key,
            kv => new NationalityPricing
            {
              BasePrice = kv.Value.BasePrice,
              AddOns = kv.Value.AddOns
            });

    SaveConfigurationToFile();
  }
  private void SaveConfigurationToFile()
  {
    try
    {
      var existingJson = File.ReadAllText(ConfigurationKeys.ConfigFileName);
      var jsonDocument = JsonDocument.Parse(existingJson);
      var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson);
      if (jsonObject == null)
      {
        Console.WriteLine("Failed to parse existing appsettings.json.");
        return;
      }
      jsonObject[ConfigurationKeys.TicketSettings] = _pricingConfig;
      var updatedJson = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
      {
        WriteIndented = true
      });
      File.WriteAllText(ConfigurationKeys.ConfigFileName, updatedJson);
      Console.WriteLine("Configuration saved successfully.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error saving configuration: {ex.Message}");
    }
  }
  private string GenerateTicketCode() => Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
}
