public class FiscalYearService
{
  public FiscalYear GetFiscalYear(int yearBS)
  {
    return new FiscalYear
    {
      StartDate = new NepaliDate { Year = yearBS, Month = 4, Day = 1 }, // Start: Shrawan 1
      EndDate = new NepaliDate { Year = yearBS + 1, Month = 3, Day = 32 } // End: Ashadh 32
    };
  }
}
