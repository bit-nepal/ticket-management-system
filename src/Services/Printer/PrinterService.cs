namespace tms.Services.Printer;

using System.Globalization;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using tms.Data;

public class PrinterService : IPrinterService
{
  public PrinterService()
  {
  }

  private string MARGIN = "   ";

  public async void PrintTicket(Ticket ticket)
  {
    printdata(ticket);
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
      e.PrintLine(""),
      e.PrintLine(""),

      // e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), false, true),
      // e.PrintLine(""),


      e.PrintLine("ENTRANCE TICKET"),
      e.PrintLine(""),
      e.SetBarcodeHeightInDots(500),
      e.SetBarWidth(BarWidth.Default),
      e.SetBarLabelPosition(BarLabelPrintPosition.None),
      e.PrintBarcode(BarcodeType.CODE128, ticket.BarCodeData == null ? "123456" : ticket.BarCodeData),
      e.PrintLine(""),
      e.PrintLine("Ticket No: " + ticket.TicketNo),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      //
      e.RightAlign(),
      e.PrintLine("Date: " + DateTime.Now.ToString("dd MMMM yyyy") + MARGIN),
      e.PrintLine("Time: " + DateTime.Now.ToString("t", DateTimeFormatInfo.InvariantInfo) + MARGIN),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      //
      e.LeftAlign(),
      e.PrintLine(ticket.CustomText is null ? "" : MARGIN + ticket.CustomText),
      e.PrintLine(MARGIN + "Nationaility: " + ticket.Nationality),
      e.PrintLine(MARGIN + "No of People: " + ticket.NoOfPeople),
      e.PrintLine(MARGIN + "Camera :" + ticket.AddOns.First(x => x.AddOnType == AddOnType.Camera).Quantity),
      e.PrintLine(MARGIN + "Video Camera:" + ticket.AddOns.First(x => x.AddOnType == AddOnType.VideoCamera).Quantity),
      e.PrintLine(MARGIN + "Entry Fee: " + ticket.TotalPrice + " NPR"),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),

      e.CenterAlign(),
      e.PrintLine(" c Hubizen Innovations"),
      //
      e.PrintLine(""),
      e.FullCutAfterFeed(0)
      )
    );
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
      e.PrintLine(""),
      e.FullCutAfterFeed(1)
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
    foreach (var addon in ticket.AddOns)
    {
      Console.WriteLine($"addon {addon.AddOnType}: quantity = {addon.Quantity}");
    }
  }

}
