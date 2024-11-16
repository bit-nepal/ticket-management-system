namespace tms.Services.Printer;

using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

public class PrinterService : IPrinterService
{
  public PrinterService()
  {
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
    var printer = new ImmediateNetworkPrinter(new ImmediateNetworkPrinterSettings() { ConnectionString = $"{hostnameOrIp}:{port}", PrinterName = "TestPrinter" });
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
}
