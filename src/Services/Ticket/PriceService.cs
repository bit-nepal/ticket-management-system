using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using tms.Configuration;
using tms.Data;
using tms.Data.Context;
using tms.Services.Printer;

public class PriceService
{
    private TicketPricingConfig _pricingConfig;
   
    public PriceService(
        IOptions<TicketPricingConfig> pricingConfig
     )
    {
        _pricingConfig = pricingConfig.Value;

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
   
}
