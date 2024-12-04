public class DailyRevenue
{
  public DateTime Date { get; set; } // Date of the revenue data
  public List<RevenueCell> RevenueCells { get; set; } = new(); // List of all cells (categories)
  public int TotalPeople { get; set; } // Aggregate count of people across all cells
  public int GrandTotalSales { get; set; } // Aggregate sales amount across all cells
}
