using System.Text.Json;
using Microsoft.Extensions.Options;
using tms.Configuration;
using tms.Data;
using tms.Services.Printer;

public class TicketService
{
  private TicketPricingConfig _pricingConfig;
  private IPrinterService _printerService;

  public TicketService(IOptions<TicketPricingConfig> pricingConfig, IPrinterService printerService)
  {
    _pricingConfig = pricingConfig.Value;
    _printerService = printerService;
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

  public void FinalizeTicket(Ticket ticket)
  {
    ticket.TotalPrice = CalculateTotalPrice(ticket);
    ticket.TicketNo = 69420;
    ticket.BarCodeData = "691420";
    // SaveTicketToDb(ticket);
    _printerService.PrintTicket(ticket);
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
}
