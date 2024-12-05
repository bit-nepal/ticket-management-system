public class YearlyRevenue
{
  public int Id { get; set; }

  public int Year { get; set; } // Nepali fiscal year

  public List<MonthlyRevenue> MonthlyRevenues { get; set; } = new();

  public int TotalPeople => MonthlyRevenues.Sum(mr => mr.TotalPeople);
  public int TotalRevenue => MonthlyRevenues.Sum(mr => mr.TotalRevenue);
}

