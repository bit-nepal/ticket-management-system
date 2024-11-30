using tms.Data;

namespace tms.Services.Printer;
public interface IPrinterService
{
  void PrintText(string text);
  Task<bool> PrintTicket(Ticket ticket);
  void Cut();
  void Feed();
  void SetPrinterMode(bool isLocalConnection);
}
