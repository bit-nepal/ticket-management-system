public class FiscalYear
{
  public NepaliDate StartDate { get; set; } // Start of the fiscal year in BS
  public NepaliDate EndDate { get; set; } // End of the fiscal year in BS
  public override string ToString() => $"{StartDate.Year}/{EndDate.Year}";
}
