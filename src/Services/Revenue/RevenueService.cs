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


  public async Task<bool> AddTicketSaleAsync(Ticket ticket)
  {
    try
    {
      var bsDate = _dateConversionService.ConvertEnglishDateToNepaliDate(DateTime.Now);

      // Get or create daily revenue
      var dailyRevenue = await GetOrCreateDailyRevenueAsync(bsDate);

      // Update ticket number range
      if (!dailyRevenue.TicketNoStart.HasValue || ticket.TicketNo < dailyRevenue.TicketNoStart.Value)
      {
        dailyRevenue.TicketNoStart = ticket.TicketNo;
      }

      if (!dailyRevenue.TicketNoEnd.HasValue || ticket.TicketNo > dailyRevenue.TicketNoEnd.Value)
      {
        dailyRevenue.TicketNoEnd = ticket.TicketNo;
      }

      // Update visitor fee
      var visitorType = GetVisitorRevenueType(ticket.Nationality, ticket.PersonType);
      var visitorCell = dailyRevenue.RevenueCells.FirstOrDefault(c => c.Type == visitorType);
      if (visitorCell == null)
      {
        visitorCell = new RevenueCell
        {
          Type = visitorType,
          NoOfPeople = ticket.NoOfPeople,
          TotalAmount = ticket.TotalPrice
        };
        dailyRevenue.RevenueCells.Add(visitorCell);
      }
      else
      {
        visitorCell.NoOfPeople += ticket.NoOfPeople;
        visitorCell.TotalAmount += ticket.TotalPrice;
      }

      // Update add-ons (camera and video camera fees)
      foreach (var addOn in ticket.AddOns)
      {
        var addOnType = GetAddOnRevenueType(ticket.Nationality, addOn.AddOnType);
        var addOnCell = dailyRevenue.RevenueCells.FirstOrDefault(c => c.Type == addOnType);
        if (addOnCell == null)
        {
          addOnCell = new RevenueCell
          {
            Type = addOnType,
            NoOfPeople = addOn.Quantity,
            TotalAmount = addOn.TotalPrice
          };
          dailyRevenue.RevenueCells.Add(addOnCell);
        }
        else
        {
          addOnCell.NoOfPeople += addOn.Quantity;
          addOnCell.TotalAmount += addOn.TotalPrice;
        }
      }

      // Save changes to the database
      return await _context.SaveChangesAsync() > 0;
    }
    catch
    {
      return false;
    }
  }




  public async Task<DailyRevenue> GetOrCreateDailyRevenueAsync(NepaliDate dateBS)
  {
    // Step 1: Ensure the YearlyRevenue exists
    var yearlyRevenue = await _context.YearlyRevenues
        .Include(yr => yr.MonthlyRevenues)
        .FirstOrDefaultAsync(yr => yr.Year == dateBS.Year);

    if (yearlyRevenue == null)
    {
      yearlyRevenue = new YearlyRevenue
      {
        Year = dateBS.Year,
        MonthlyRevenues = new List<MonthlyRevenue>()
      };
      _context.YearlyRevenues.Add(yearlyRevenue);
      await _context.SaveChangesAsync(); // Save to get YearlyRevenue.Id
    }

    // Step 2: Ensure the MonthlyRevenue exists
    var monthlyRevenue = yearlyRevenue.MonthlyRevenues
        .FirstOrDefault(mr => mr.Month == (NepaliMonth)dateBS.Month);

    if (monthlyRevenue == null)
    {
      monthlyRevenue = new MonthlyRevenue
      {
        Year = dateBS.Year,
        Month = (NepaliMonth)dateBS.Month,
        YearlyRevenueId = yearlyRevenue.Id,
        DailyRevenues = new List<DailyRevenue>()
      };
      _context.MonthlyRevenues.Add(monthlyRevenue);
      await _context.SaveChangesAsync(); // Save to get MonthlyRevenue.Id
    }

    // Step 3: Ensure the DailyRevenue exists
    var dailyRevenue = await _context.DailyRevenues
        .Include(dr => dr.RevenueCells)
        .FirstOrDefaultAsync(dr => dr.DateBS.Year == dateBS.Year &&
                                   dr.DateBS.Month == dateBS.Month &&
                                   dr.DateBS.Day == dateBS.Day);
    if (dailyRevenue == null)
    {
      Console.WriteLine("ADDING DAILY REVEUE CAUSE IT DOESNT EXIST");
      dailyRevenue = new DailyRevenue
      {
        DateAD = _dateConversionService.ConvertNepaliDateToEnglishDate(dateBS),
        DateBS = dateBS,
        MonthlyRevenueId = monthlyRevenue.Id
      };
      _context.DailyRevenues.Add(dailyRevenue);
      await _context.SaveChangesAsync();
    }
    return dailyRevenue;
  }

  public RevenueType GetVisitorRevenueType(Nationality nationality, PersonType personType)
  {
    return (nationality, personType) switch
    {
      (Nationality.Foreigner, _) => RevenueType.VisitorFee_Foreigner,
      (Nationality.SAARCMember, _) => RevenueType.VisitorFee_SAARC,
      (Nationality.Nepali, PersonType.Student) => RevenueType.VisitorFee_Student,
      (Nationality.Nepali, _) => RevenueType.VisitorFee_Nepali,
      _ => throw new InvalidOperationException("Invalid visitor type.")
    };
  }

  public RevenueType GetAddOnRevenueType(Nationality nationality, AddOnType addOnType)
  {
    return (nationality, addOnType) switch
    {
      (Nationality.Foreigner, AddOnType.Camera) => RevenueType.CameraFee_Foreigner,
      (Nationality.SAARCMember, AddOnType.Camera) => RevenueType.CameraFee_SAARC,
      (Nationality.Nepali, AddOnType.Camera) => RevenueType.CameraFee_Nepali,
      (Nationality.Foreigner, AddOnType.VideoCamera) => RevenueType.VideoCameraFee_Foreigner,
      (Nationality.SAARCMember, AddOnType.VideoCamera) => RevenueType.VideoCameraFee_SAARC,
      (Nationality.Nepali, AddOnType.VideoCamera) => RevenueType.VideoCameraFee_Nepali,
      _ => throw new InvalidOperationException("Invalid add-on type.")
    };
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
  public async Task<MonthlyRevenue> GetMonthlyRevenueAsync(int yearBS, NepaliMonth monthBS, int endDay)
  {
    var dailyRevenues = await _context.DailyRevenues
      .Where(dr =>
          dr.DateBS.Year == yearBS &&
          dr.DateBS.Month == (int)monthBS &&
          dr.DateBS.Day <= endDay
        )
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
  public async Task<YearlyRevenue> GetYearlyRevenueAsync(int yearBS, NepaliMonth endMonth)
  {
    var monthlyRevenues = await _context.MonthlyRevenues
        .Where(mr =>
                  mr.Year == yearBS &&
                  mr.Month <= endMonth
              )
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
