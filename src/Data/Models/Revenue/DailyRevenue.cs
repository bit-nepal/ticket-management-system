public class DailyRevenue
{
  public int Id { get; set; }

  public DateTime DateAD { get; set; } // Gregorian date
  public NepaliDate DateBS { get; set; } // Nepali date

  public List<RevenueCell> RevenueCells { get; set; } = new();

  public int TotalPeople => RevenueCells.Sum(rc => rc.NoOfPeople);
  public int TotalRevenue => RevenueCells.Sum(rc => rc.TotalAmount);

  // Foreign key to MonthlyRevenue
  public int MonthlyRevenueId { get; set; }
  public MonthlyRevenue MonthlyRevenue { get; set; } = null!;
}

