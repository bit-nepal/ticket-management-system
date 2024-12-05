using tms.Data;
using tms.Data.Context;
using Microsoft.EntityFrameworkCore;

public class RevenueService
{
  private readonly LocalDbContext _context;
  private readonly DateConversionService _dateConversionService;
  private readonly FiscalYearService _fiscalYearService;

  public RevenueService(LocalDbContext context, DateConversionService dateConversionService, FiscalYearService fiscalYearService)
  {
    _context = context;
    _dateConversionService = dateConversionService;
    _fiscalYearService = fiscalYearService;
  }


  public async Task AddTicketSaleAsync(Ticket ticket)
  {
    Console.WriteLine("================== ADDING TICKET TO REVENUE " + ticket.TimeStamp.Date);
    // ticket.TimeStamp.Date = DateTime.Now;
    var bsDate = _dateConversionService.ConvertEnglishDateToNepaliDate(DateTime.Now);
    // Get or create daily revenue
    Console.WriteLine("================== ADDING TICKET TO REVENUE " + bsDate);
    var dailyRevenue = await GetOrCreateDailyRevenueAsync(bsDate);
    // Update revenue cells
    foreach (var addOn in ticket.AddOns)
    {
      var cell = dailyRevenue.RevenueCells.FirstOrDefault(c => c.Type == (RevenueType)addOn.AddOnType);
      if (cell == null)
      {
        cell = new RevenueCell
        {
          Type = (RevenueType)addOn.AddOnType,
          NoOfPeople = addOn.Quantity,
          TotalAmount = addOn.TotalPrice
        };
        dailyRevenue.RevenueCells.Add(cell);
      }
      else
      {
        cell.NoOfPeople += addOn.Quantity;
        cell.TotalAmount += addOn.TotalPrice;
      }
    }
    await _context.SaveChangesAsync();
  }



  public async Task<DailyRevenue> GetOrCreateDailyRevenueAsync(NepaliDate dateBS)
  {
    var dateAD = _dateConversionService.ConvertNepaliDateToEnglishDate(dateBS);
    var dailyRevenue = await _context.DailyRevenues
        .Include(dr => dr.RevenueCells)
        .FirstOrDefaultAsync(dr => dr.DateAD == dateAD);

    if (dailyRevenue == null)
    {
      dailyRevenue = new DailyRevenue { DateAD = dateAD, DateBS = dateBS };
      _context.DailyRevenues.Add(dailyRevenue);
    }
    return dailyRevenue;
  }


  public async Task<MonthlyRevenue> GetMonthlyRevenueAsync(int yearBS, NepaliMonth monthBS)
  {
    var dailyRevenues = await _context.DailyRevenues
        .Where(dr => dr.DateBS.Year == yearBS && dr.DateBS.Month == (int)monthBS)
        .Include(dr => dr.RevenueCells)
        .ToListAsync();

    return new MonthlyRevenue
    {
      Year = yearBS,
      Month = monthBS,
      DailyRevenues = dailyRevenues
    };
  }


  public async Task<YearlyRevenue> GetYearlyRevenueAsync(int yearBS)
  {
    var monthlyRevenues = await _context.MonthlyRevenues
        .Where(mr => mr.Year == yearBS)
        .Include(mr => mr.DailyRevenues)
        .ThenInclude(dr => dr.RevenueCells)
        .ToListAsync();

    return new YearlyRevenue
    {
      Year = yearBS,
      MonthlyRevenues = monthlyRevenues
    };
  }
}
