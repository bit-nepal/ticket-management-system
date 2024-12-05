public class YearlyRevenue
{
  public int Id { get; set; }

  public int Year { get; set; } // Nepali fiscal year

  public List<MonthlyRevenue> MonthlyRevenues { get; set; } = new();
    public int? GetTotalPeople(RevenueType revenueType)
    {
        int result = MonthlyRevenues
            .SelectMany(mr => mr.DailyRevenues)
            .SelectMany(dr => dr.RevenueCells)
            .Where(rc => rc.Type == revenueType)
            .Sum(rc => rc.NoOfPeople);
        return result;
    }

    public int? GetTotalRevenue(RevenueType revenueType)
    {
        int result = MonthlyRevenues
            .SelectMany(mr => mr.DailyRevenues)
            .SelectMany(dr => dr.RevenueCells)
            .Where(rc => rc.Type == revenueType)
            .Sum(rc => rc.TotalAmount);
        return result;
    }
    public int TotalPeople => MonthlyRevenues.Sum(mr => mr.TotalPeople);
  public int TotalRevenue => MonthlyRevenues.Sum(mr => mr.TotalRevenue);
}

