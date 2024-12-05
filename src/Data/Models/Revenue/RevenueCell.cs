public class RevenueCell
{
  public int Id { get; set; }

  public RevenueType Type { get; set; } // Enum: NepaliVisitor, SAARCVisitor, etc.
  public int NoOfPeople { get; set; }
  public int TotalAmount { get; set; }

  // Foreign key to DailyRevenue
  public int DailyRevenueId { get; set; }
  public DailyRevenue DailyRevenue { get; set; } = null!;
}

