public class MonthlyRevenue
{
  public int Id { get; set; }

  public NepaliMonth Month { get; set; }
  public int Year { get; set; }

  public List<DailyRevenue> DailyRevenues { get; set; } = new();

  public int TotalPeople => DailyRevenues.Sum(dr => dr.TotalPeople);
  public int TotalRevenue => DailyRevenues.Sum(dr => dr.TotalRevenue);

  // Foreign key to YearlyRevenue
  public int YearlyRevenueId { get; set; }
  public YearlyRevenue YearlyRevenue { get; set; } = null!;
}

