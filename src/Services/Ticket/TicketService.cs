using System.Text.Json;
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
      LocalDbContext localDbContext,
      DateConversionService dateConversionService)
  {
    _pricingConfig = pricingConfig.Value;
    _printerService = printerService;
    _localDbContext = localDbContext;
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

  public async Task<bool> FinalizeTicket(Ticket ticket)
  {
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

      if (!await CreateTicket(ticket)) { transaction.Dispose(); return false; }
      ticket.TicketNo = ticket.Id;
      if (!await _revenueService.AddTicketSaleAsync(ticket)) { transaction.Dispose(); return false; }
      if (!await _printerService.PrintTicket(ticket)) { transaction.Commit(); return false; }

      transaction.Commit();
      return true;
    }
    catch (Exception e)
    {
      transaction.Dispose();
      return false;
    }
  }
  public async Task<bool> CreateTicket(Ticket ticket)
  {
    _localDbContext.Add(ticket);
    return await _localDbContext.SaveChangesAsync() > 0;
  }
  public TicketPricingConfig GetPricingConfig()
  {
    return _pricingConfig;
  }

  public async Task<Ticket?> VerifyTicket(string BarCode)
  {
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
