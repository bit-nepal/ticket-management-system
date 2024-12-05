public class RevenueCell
{
  public RevenueType RevenueType { get; set; } // e.g., VisitorFee_Foreigner, CameraFee_SAARC
  public int NoOfPeople { get; set; } // Number of people for this category
  public int TotalSales { get; set; } // Total sales amount for this category
}
