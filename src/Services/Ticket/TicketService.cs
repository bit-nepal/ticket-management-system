using System.Text.Json;
using Microsoft.Extensions.Options;
using tms.Configuration;

public class TicketService
{
  private TicketPricingConfig _pricingConfig;

  public TicketService(IOptions<TicketPricingConfig> pricingConfig)
  {
    _pricingConfig = pricingConfig.Value;
  }

  public int GetBaseTicketPrice(Nationality nationality, PersonType personType)
  {
    return _pricingConfig.Nationalities[nationality].BasePrice[personType];
  }
  public TicketPricingConfig GetPricingConfig()
  {
    return _pricingConfig;
  }

  public void UpdatePricingConfig(TicketPricingConfig updatedConfig)
  {
    _pricingConfig = updatedConfig;
  }
  public int GetAddOnFee(Nationality nationality, AddOnType addOn)
  {
    var addOns = _pricingConfig.Nationalities[nationality].AddOns;
    return addOns.TryGetValue(addOn, out int fee) ? fee : 0;
  }

  public int CalculateTotalTicketPrice(Nationality nationality, PersonType personType, List<AddOnType> addOns)
  {
    var basePrice = GetBaseTicketPrice(nationality, personType);
    var addOnFees = addOns
        .Select(addOn => GetAddOnFee(nationality, addOn))
        .Sum();
    return basePrice + addOnFees;
  }

  // public EditableTicketPricingConfig GetEditablePricingConfig()
  // {
  //   var editableConfig = new EditableTicketPricingConfig();
  //
  //   foreach (var nationality in _pricingConfig.Nationalities)
  //   {
  //     editableConfig.Nationalities[nationality.Key] = new EditableNationalityPricing
  //     {
  //       BasePrice = new Dictionary<PersonType, int>(nationality.Value.BasePrice),
  //       AddOns = new Dictionary<AddOnType, int>(nationality.Value.AddOns)
  //     };
  //   }
  //
  //   return editableConfig;
  // }
  //
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
      // Read the existing JSON
      var existingJson = File.ReadAllText(ConfigurationKeys.ConfigFileName);

      // Parse the existing JSON into a dynamic object
      var jsonDocument = JsonDocument.Parse(existingJson);
      var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson);

      if (jsonObject == null)
      {
        Console.WriteLine("Failed to parse existing appsettings.json.");
        return;
      }

      // Update only the TicketSettings section
      jsonObject["TicketSettings"] = _pricingConfig;

      // Serialize the updated JSON back to the file
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
