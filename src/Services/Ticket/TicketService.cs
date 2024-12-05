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

  public TicketService(
      IOptions<TicketPricingConfig> pricingConfig,
      IPrinterService printerService,
  RevenueService revenueService,
      LocalDbContext localDbContext)
  {
    _pricingConfig = pricingConfig.Value;
    _printerService = printerService;
    _localDbContext = localDbContext;
    _revenueService = revenueService;
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
    ticket.TotalPrice = CalculateTotalPrice(ticket);
    ticket.BarCodeData = GenerateTicketCode();
    ticket.NepaliDate = "2";
    bool result;
    Console.WriteLine(GenerateTicketCode());
    if (await CreateTicket(ticket))
    {
      ticket.TicketNo = ticket.Id;
      Console.WriteLine("PRINT" + ticket.TimeStamp.Date);
      result = await _printerService.PrintTicket(ticket);
    }
    await _revenueService.AddTicketSaleAsync(ticket);
    return result;
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
