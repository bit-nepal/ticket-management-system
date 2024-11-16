public class EditableTicketPricingConfig
{
  public Dictionary<Nationality, EditableNationalityPricing> Nationalities { get; set; } = new();
}

public class EditableNationalityPricing
{
  public List<EditablePrice> BasePrices { get; set; } = new();
  public List<EditableAddOn> AddOns { get; set; } = new();
}

public class EditablePrice
{
  public PersonType PersonType { get; set; }
  public int Price { get; set; }
}

public class EditableAddOn
{
  public AddOnType AddOnType { get; set; }
  public int Price { get; set; }
}

