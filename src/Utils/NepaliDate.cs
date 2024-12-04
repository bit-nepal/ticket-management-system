public class NepaliDate
{
  public int Year { get; set; } // BS Year
  public int Month { get; set; } // BS Month (1-12)
  public int Day { get; set; } // BS Day (1-32, depending on the month)

  public override string ToString() => $"{Year}-{Month:D2}-{Day:D2}";
}
