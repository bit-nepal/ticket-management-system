public class DailyRevenue
{
  public int Id { get; set; }

  public DateTime DateAD { get; set; } // Gregorian date
  public NepaliDate DateBS { get; set; } // Nepali date
  public int? TicketNoStart { get; set; } // First ticket number of the day
  public int? TicketNoEnd { get; set; } // Last ticket number of the day

  public int? GetTotalPeople(RevenueType revenueType)
  {
    int result = RevenueCells.Where(x => x.Type == revenueType).Sum(rc => rc.NoOfPeople);
    return result;
  }
  public int? GetTotalRevenue(RevenueType revenueType)
  {
    if (revenueType == RevenueType.CameraFee_Nepali)
    {

    }
    int result = RevenueCells.Where(x => x.Type == revenueType).Sum(rc => rc.TotalAmount);
    if (revenueType == RevenueType.CameraFee_Nepali)
    {
      var r = RevenueCells.Where(x => x.Type == revenueType);
    }
    return result;
  }
  public List<RevenueCell> RevenueCells { get; set; } = new();

  public int TotalPeople => RevenueCells.Sum(rc => rc.NoOfPeople);
  public int TotalRevenue => RevenueCells.Sum(rc => rc.TotalAmount);

  // Foreign key to MonthlyRevenue
  public int MonthlyRevenueId { get; set; }
  public MonthlyRevenue MonthlyRevenue { get; set; } = null!;
}

