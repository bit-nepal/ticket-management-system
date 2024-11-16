public class TicketPricingConfig
{
  public Dictionary<Nationality, NationalityPricing> Nationalities { get; set; } = new();
}

public class NationalityPricing
{
  public Dictionary<PersonType, int> BasePrice { get; set; } // General and Student
  public Dictionary<AddOnType, int> AddOns { get; set; }// Add-ons like Camera, Guide
}
