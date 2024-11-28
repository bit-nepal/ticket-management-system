namespace tms.Services.Printer;

using System.Globalization;
using System.Runtime.InteropServices;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using tms.Data;

public class PrinterService : IPrinterService
{
  private readonly PrinterConfigurationService _printerConfigService;
  public PrinterService(PrinterConfigurationService printerConfigurationService)
  {
    _printerConfigService = printerConfigurationService;
  }

  private string MARGIN = "   ";
  public bool isLocalConnection { get; set; }

  public void SetPrinterMode(bool isLocalConnection)
  {
    this.isLocalConnection = isLocalConnection;
  }
  public async void PrintTicket(Ticket ticket)
  {
    printdata(ticket);
    if (!isLocalConnection)
    {
      Console.WriteLine("Printing Remotely");
      var hostnameOrIp = _printerConfigService.GetHost();
      var port = _printerConfigService.GetPort();
      var printer = new ImmediateNetworkPrinter(
          new ImmediateNetworkPrinterSettings()
          {
            ConnectionString = $"{hostnameOrIp}:{port}",
            PrinterName = "TestPrinter"
          }
        );
      await printer.WriteAsync(getTicketBytes(ticket));
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      Console.WriteLine("Printing Locally on Windows");
      var printer = new SerialPrinter(portName: _printerConfigService.GetComPort(), baudRate: 115200);
      printer.Write(getTicketBytes(ticket));
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
      Console.WriteLine("Printing Locally Linux");
      var printer = new FilePrinter(filePath: _printerConfigService.GetComPort());
      printer.Write(getTicketBytes(ticket));
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
      Console.WriteLine("Printing Locally OSX");
    }
    else
    {
      Console.WriteLine("ELSE");
    }
  }

  public async void PrintText(string text)
  {
    Console.WriteLine("Printing : " + text);
    var hostnameOrIp = "192.168.1.222";
    var port = 9100;
    var printer = new ImmediateNetworkPrinter(
        new ImmediateNetworkPrinterSettings()
        {
          ConnectionString = $"{hostnameOrIp}:{port}",
          PrinterName = "TestPrinter"
        }
      );
    var e = new EPSON();
    await printer.WriteAsync(
    ByteSplicer.Combine(
      e.CenterAlign(),
      e.PrintLine(text),
      e.PrintLine("")
    )
  );
  }

  private void printdata(Ticket ticket)
  {
    Console.WriteLine("Printing Data before Printing");
    Console.WriteLine("Ticket No: " + ticket.TicketNo);
    Console.WriteLine("nationality: " + ticket.Nationality);
    Console.WriteLine("person type: " + ticket.PersonType);
    Console.WriteLine("group visit: " + ticket.IsGroupVisit);
    Console.WriteLine("no. of people: " + ticket.NoOfPeople);
    Console.WriteLine("group name: " + ticket.CustomText);
    Console.WriteLine("Barcode: " + ticket.BarCodeData);
    Console.WriteLine("Cost: " + ticket.TotalPrice);

    foreach (var addon in ticket.AddOns)
    {
      Console.WriteLine($"addon {addon.AddOnType}: quantity = {addon.Quantity}");
    }
  }

  public async void Cut()
  {
    var hostnameOrIp = "192.168.1.222";
    var port = 9100;
    var printer = new ImmediateNetworkPrinter(
        new ImmediateNetworkPrinterSettings()
        {
          ConnectionString = $"{hostnameOrIp}:{port}",
          PrinterName = "TestPrinter"
        }
      );

    var e = new EPSON();
    await printer.WriteAsync(
    ByteSplicer.Combine(
          e.FullCut()
      )
    );
  }
  public async void Feed()
  {
    Console.WriteLine("Manually feeding");
    var hostnameOrIp = "192.168.1.222";
    var port = 9100;
    var printer = new ImmediateNetworkPrinter(
        new ImmediateNetworkPrinterSettings()
        {
          ConnectionString = $"{hostnameOrIp}:{port}",
          PrinterName = "TestPrinter"
        }
      );

    var e = new EPSON();
    await printer.WriteAsync(
    ByteSplicer.Combine(
          e.PrintLine("")
      )
    );
  }

  private byte[] getTicketBytes(Ticket ticket)
  {
    var e = new EPSON();
    return ByteSplicer.Combine(
     e.LeftAlign(),
     // e.PrintQRCode("amrit-p.com.np", TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.TINY),
     e.CenterAlign(),
     e.SetStyles(PrintStyle.Bold),
     e.PrintLine("NATIONAL ART MUSEUM"),

     e.SetStyles(PrintStyle.None),
     e.PrintLine("Bhaktapur, Nepal"),
     e.PrintLine(""),
     e.PrintLine(""),
     e.PrintLine(""),

     // e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), false, true),
     // e.PrintLine(""),

     e.SetStyles(PrintStyle.Bold),
     e.PrintLine("ENTRANCE TICKET"),
     e.SetStyles(PrintStyle.None),
     e.PrintLine(""),
     e.PrintLine(""),
     e.PrintLine("[ ]  Painting  Section "),
     e.PrintLine("[ ] Woodcraft  Section "),
     e.PrintLine("[ ] Metalcraft Section "),
     e.PrintLine(""),
     e.PrintLine(""),
     e.SetBarcodeHeightInDots(250),
     e.SetBarWidth(BarWidth.Default),
     e.SetBarLabelPosition(BarLabelPrintPosition.None),
     e.PrintBarcode(BarcodeType.CODE128, ticket.BarCodeData == null ? "123456" : ticket.BarCodeData),
     e.PrintLine("Ticket No: " + ticket.TicketNo),
     e.PrintLine(""),
     e.PrintLine(""),
     //
     e.RightAlign(),
     e.PrintLine("Date: " + DateTime.Now.ToString("dd MMMM yyyy") + MARGIN),
     e.PrintLine("Time: " + DateTime.Now.ToString("t", DateTimeFormatInfo.InvariantInfo) + MARGIN),
     e.PrintLine(""),
     e.PrintLine(""),
     e.PrintLine(""),
     //
     e.LeftAlign(),
     // e.PrintLine(ticket.CustomText is null ? "" : MARGIN + ticket.CustomText),
     e.PrintLine(MARGIN + "Name:" + ticket.CustomText),

     e.Print(MARGIN + "Nationality: "),
     e.Print(ticket.Nationality == Nationality.SAARCMember ? "SAARC Member" : ticket.Nationality + ""),
     e.PrintLine(ticket.Nationality == Nationality.Nepali ? " " + ticket.PersonType : ""),

     e.PrintLine(MARGIN + "No of People: " + ticket.NoOfPeople),
     e.PrintLine(MARGIN + "Camera:" + ticket.AddOns.First(x => x.AddOnType == AddOnType.Camera).Quantity),
     e.PrintLine(MARGIN + "Video Camera:" + ticket.AddOns.First(x => x.AddOnType == AddOnType.VideoCamera).Quantity),
     e.PrintLine(MARGIN + "Entrance Fee: Rs " + ticket.TotalPrice),
     e.PrintLine(""),
     e.PrintLine(""),
     e.PrintLine(""),
     e.PrintLine(""),

     e.CenterAlign(),
     e.SetStyles(PrintStyle.Condensed),
     e.PrintLine("(c) Hubizen"),
     //
     e.PrintLine(""),
     e.FullCutAfterFeed(0),

     e.LeftAlign(),
     e.PrintLine(MARGIN + "Ticket No: " + ticket.TicketNo),
     e.PrintLine(MARGIN + "Entrance Fee: Rs " + ticket.TotalPrice),
     e.FullCutAfterFeed(0)
   );
  }
}
