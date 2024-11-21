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
      e.CenterAlign(),
      e.SetStyles(PrintStyle.Bold),
      e.PrintLine("NATIONAL MUSEUM"),

      e.SetStyles(PrintStyle.None),
      e.PrintLine("Chhauni, Nepal"),
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
      e.PrintBarcode(BarcodeType.ITF, ticket.BarCodeData == null ? "123456" : ticket.BarCodeData),
      e.PrintLine(""),
      e.PrintLine("Ticket No: " + ticket.TicketNo),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      //
      e.RightAlign(),
      e.PrintLine("Date: " + DateTime.Now.ToString("dd MMMM yyyy")),
      e.PrintLine("Time: " + DateTime.Now.ToString("t", DateTimeFormatInfo.InvariantInfo)),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      //
      e.LeftAlign(),
      e.PrintLine(ticket.CustomText is null ? "" : ticket.CustomText),
      e.PrintLine("Nationaility: " + ticket.Nationality),
      e.PrintLine("No of People: " + ticket.NoOfPeople),
      e.PrintLine("Camera :" + ticket.AddOns.First(x => x.AddOnType == AddOnType.Camera).Quantity),
      e.PrintLine("Video Camera:" + ticket.AddOns.First(x => x.AddOnType == AddOnType.VideoCamera).Quantity),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),
      e.PrintLine(""),

      e.CenterAlign(),
      e.PrintLine("c Hubizen Innovations Pvt. Ltd."),
      //
      e.PrintLine(""),
      e.FullCutAfterFeed(0)
      // e.FullCutAfterFeed(1)
      )
    );

    //   var hostnameOrIp = "192.168.1.222";
    //   var port = 9100;
    //   var printer = new ImmediateNetworkPrinter(new ImmediateNetworkPrinterSettings() { ConnectionString = $"{hostnameOrIp}:{port}", PrinterName = "TestPrinter" });
    //   var e = new EPSON();
    //   if (printer is null)
    //   {
    //     Console.WriteLine("It is Null");
    //     return;
    //   }
    //   await printer.WriteAsync(
    //   ByteSplicer.Combine(
    //     e.CenterAlign(),
    //
    //     e.SetStyles(PrintStyle.Bold),
    //     e.PrintLine("NATIONAL MUSEUM"),
    //
    //     e.SetStyles(PrintStyle.None),
    //     e.PrintLine("Chhauni, Nepal"),
    //     e.PrintLine(""),
    //     e.PrintLine(""),
    //     e.PrintLine(""),
    //
    //     // e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), false, true),
    //     // e.PrintLine(""),
    //
    //
    //     e.PrintLine("ENTRANCE TICKET"),
    //     e.PrintLine(""),
    //     e.SetBarcodeHeightInDots(500),
    //     e.SetBarWidth(BarWidth.Default),
    //     e.SetBarLabelPosition(BarLabelPrintPosition.None),
    //     e.PrintBarcode(BarcodeType.ITF, ticket.BarCodeData == null ? "123456" : ticket.BarCodeData),
    //     e.PrintLine(""),
    //     e.PrintLine("Ticket No: " + ticket.TicketNo),
    //     e.PrintLine(""),
    //     e.PrintLine(""),
    //
    //     e.RightAlign(),
    //     e.PrintLine("Date: " + DateTime.Now.ToString("dd MMMM yyyy")),
    //     e.PrintLine("Time: " + DateTime.Now.ToString("dddd t", DateTimeFormatInfo.InvariantInfo)),
    //     // e.PrintLine("Camera :" + ticket.AddOns.First(x=> x.AddOnType == AddOnType.Camera).Quantity),
    //     e.PrintLine(""),
    //     e.PrintLine(""),
    //
    //     e.LeftAlign(),
    //     e.Print(ticket.CustomText),
    //     e.PrintLine("Nationaility: " + ticket.Nationality),
    //     e.PrintLine("No of People: " + ticket.NoOfPeople),
    //     e.PrintLine(""),
    //     e.PrintLine(""),
    //
    //     e.CenterAlign(),
    //     e.PrintLine("c Hubizen Innovations Pvt. Ltd."),
    //
    //     e.PrintLine(""),
    //     e.FullCutAfterFeed(1)
    //   )
    // );
  }
  public void PrintText(string text)
  {
    Console.WriteLine("Printing : " + text);
    // Generate ESC/POS commands
    // Send commands to printer (this may involve a method that directly sends raw data to a connected printer)
    SendToPrinter(text);
  }

  private async void SendToPrinter(String message)
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
      e.CenterAlign(),
      e.PrintLine(message),
      // e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), false, true),
      // e.PrintLine(""),
      // e.SetBarcodeHeightInDots(500),
      // e.SetBarWidth(BarWidth.Default),
      // e.SetBarLabelPosition(BarLabelPrintPosition.None),
      // e.PrintBarcode(BarcodeType.ITF, "0123456789"),
      // e.PrintLine(""),
      // e.PrintLine("B&H PHOTO & VIDEO"),
      // e.PrintLine("420 NINTH AVE."),
      // e.PrintLine("NEW YORK, NY 10001"),
      // e.PrintLine("(212) 502-6380 - (800)947-9975"),
      // e.SetStyles(PrintStyle.Underline),
      // e.PrintLine("www.bhphotovideo.com"),
      // e.SetStyles(PrintStyle.None),
      // e.PrintLine(""),
      // e.LeftAlign(),
      // e.PrintLine("Order: 123456789        Date: 02/01/19"),
      // e.PrintLine(""),
      // e.PrintLine(""),
      // e.SetStyles(PrintStyle.FontB),
      // e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
      // e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
      // e.PrintLine("----------------------------------------------------------------"),
      // e.RightAlign(),
      // e.PrintLine("SUBTOTAL         89.95"),
      // e.PrintLine("Total Order:         89.95"),
      // e.PrintLine("Total Payment:         89.95"),
      // e.PrintLine(""),
      // e.LeftAlign(),
      // e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
      // e.PrintLine("SOLD TO:                        SHIP TO:"),
      // e.SetStyles(PrintStyle.FontB),
      // e.PrintLine("  FIRSTN LASTNAME                 FIRSTN LASTNAME"),
      // e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
      // e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
      // e.PrintLine("  (123)456-7890                   (123)456-7890"),
      // e.PrintLine("  CUST: 87654321"),
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
