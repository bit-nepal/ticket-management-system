using tms.Data;

namespace tms.Services.Printer;
public interface IPrinterService
{
  void PrintText(string text);
  void PrintTicket(Ticket ticket);
}
