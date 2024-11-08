using System.Text;

namespace tms.Services.Printer;
public class EscPosParser
{
  public byte[] GenerateCommands(string text)
  {
    // Simple example: add line feed after each text line
    var commandBuilder = new StringBuilder();
    commandBuilder.Append("\x1B\x40"); // Initialize the printer
    commandBuilder.Append(text);
    commandBuilder.Append("\x0A"); // Line feed

    return Encoding.ASCII.GetBytes(commandBuilder.ToString());
  }
}
