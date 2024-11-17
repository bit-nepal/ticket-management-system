public class AddOn
{
  public AddOnType AddOnType { get; set; } // Enum for add-on type
  public int Quantity { get; set; } // Number of add-ons (e.g., 2 Cameras)
  public int TotalPrice { get; set; } // Total price for this add-on

  public bool IsSelected
  {
    get => Quantity > 0;
    set => Quantity = value ? 1 : 0;
  }
}
