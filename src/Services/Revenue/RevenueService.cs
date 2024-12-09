using tms.Data;
using tms.Data.Context;
using Microsoft.EntityFrameworkCore;

public class RevenueService
{
  private readonly DateConversionService _dateConversionService;
  private readonly FiscalYearService _fiscalYearService;
    private LocalDbContext _localDbContext;
    private PriceService _priceService;

    

    public RevenueService(PriceService priceService, LocalDbContext localDbContext, DateConversionService dateConversionService, FiscalYearService fiscalYearService)
  {
        _localDbContext = localDbContext;
        _dateConversionService = dateConversionService;
    _fiscalYearService = fiscalYearService;
        _priceService = priceService;
    }


    public async Task<bool> AddTicketSaleAsync(Ticket ticket)
    {
        try
        {
            Console.WriteLine("================== ADDING REVENUE AND FINALIZING TICKET =====================");
            var bsDate = _dateConversionService.getCurrentDate();
            ticket.NepaliDate = new NepaliDate { Year = 2080, Month = 1, Day = 15 };

            // Get or create daily revenue
            var dailyRevenue = await GetOrCreateDailyRevenueAsync(bsDate);
            if (dailyRevenue == null)
            {
                throw new InvalidOperationException("Failed to retrieve or create the daily revenue record.");
            }

            // Update ticket number range
            if (!dailyRevenue.TicketNoStart.HasValue || ticket.TicketNo < dailyRevenue.TicketNoStart.Value)
            {
                dailyRevenue.TicketNoStart = ticket.TicketNo;
            }
            if (!dailyRevenue.TicketNoEnd.HasValue || ticket.TicketNo > dailyRevenue.TicketNoEnd.Value)
            {
                dailyRevenue.TicketNoEnd = ticket.TicketNo;
            }

            Console.WriteLine("==================");

            // Update visitor fee
            var visitorType = GetVisitorRevenueType(ticket.Nationality, ticket.PersonType);
            var visitorCell = dailyRevenue.RevenueCells.FirstOrDefault(c => c.Type == visitorType);
            Console.WriteLine($"Visitor Type: {visitorType}");
            Console.WriteLine($"No of People: {ticket.NoOfPeople}");
            Console.WriteLine($"Total Price: {ticket.TotalPrice}");

            if (visitorCell == null)
            {
                visitorCell = new RevenueCell
                {
                    Type = visitorType,
                    NoOfPeople = ticket.NoOfPeople,
                    TotalAmount = ticket.NoOfPeople * _priceService.GetBaseTicketPrice(ticket.Nationality, ticket.PersonType)
                };
                dailyRevenue.RevenueCells.Add(visitorCell);
            }
            else
            {
                visitorCell.NoOfPeople += ticket.NoOfPeople;
                visitorCell.TotalAmount += ticket.NoOfPeople * _priceService.GetBaseTicketPrice(ticket.Nationality, ticket.PersonType);
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

            Console.WriteLine("==================");

            // Save changes to the database
            var result = await _localDbContext.SaveChangesAsync();
            if (result <= 0)
            {
                throw new DbUpdateConcurrencyException("The database operation did not affect any rows. Potential concurrency issue.");
            }

            Console.WriteLine($"==================DONE ADDING REVENUE AND FINALIZING TICKET : {result > 0}=====================");
            return result > 0;
        }
        catch (DbUpdateConcurrencyException e)
        {
            Console.WriteLine($"Concurrency Error: {e.Message}\nStackTrace: {e.StackTrace}");
            if (e.Entries != null && e.Entries.Count > 0)
            {
                foreach (var entry in e.Entries)
                {
                    Console.WriteLine($"Failed Entity Type: {entry.Entity.GetType().Name}");
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    if (databaseValues != null)
                    {
                        Console.WriteLine("Database values:");
                        foreach (var property in databaseValues.Properties)
                        {
                            Console.WriteLine($"- {property.Name}: {databaseValues[property]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The entity no longer exists in the database.");
                    }
                }
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}\nStackTrace: {e.StackTrace}");
            if (e.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
            }
            return false;
        }
    }





    public async Task<DailyRevenue> GetOrCreateDailyRevenueAsync(NepaliDate dateBS)
  {
    //var _context = _localDbContextFactory.CreateDbContext();

    // Step 1: Ensure the YearlyRevenue exists
    var yearlyRevenue = await _localDbContext.YearlyRevenues
    .Include(yr => yr.MonthlyRevenues)
    .FirstOrDefaultAsync(yr => yr.Year == dateBS.Year);

    if (yearlyRevenue == null)
    {
      yearlyRevenue = new YearlyRevenue
      {
        Year = dateBS.Year,
        MonthlyRevenues = new List<MonthlyRevenue>()
      };
      _localDbContext.YearlyRevenues.Add(yearlyRevenue);
      await _localDbContext.SaveChangesAsync(); // Save to get YearlyRevenue.Id
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
      _localDbContext.MonthlyRevenues.Add(monthlyRevenue);
      await _localDbContext.SaveChangesAsync(); // Save to get MonthlyRevenue.Id
    }

    // Step 3: Ensure the DailyRevenue exists
    var dailyRevenue = await _localDbContext.DailyRevenues
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
      _localDbContext.DailyRevenues.Add(dailyRevenue);
      await _localDbContext.SaveChangesAsync();
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
        //var _context = _localDbContextFactory.CreateDbContext();
        var dailyRevenues = await _localDbContext.DailyRevenues
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
       var dailyRevenues = await _localDbContext.DailyRevenues
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
        //var _context = _localDbContextFactory.CreateDbContext();

        var monthlyRevenues = await _localDbContext.MonthlyRevenues
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
        //var _context = _localDbContextFactory.CreateDbContext();

        var monthlyRevenues = await _localDbContext.MonthlyRevenues
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
