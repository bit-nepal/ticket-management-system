using System.ComponentModel.DataAnnotations.Schema;
namespace tms.Data;

public class Ticket
{
  [Column("id")]
  public int Id { get; set; }
  public int TicketNo { get; set; }
  public DateTime TimeStamp { get; set; }
  public String NepaliDate { get; set; } = null!;
  public String BarCodeData { get; set; } = null!;
  public Nationality Nationality { get; set; }
  public PersonType PersonType { get; set; }
  public int NoOfPeople { get; set; }
  public int TotalPrice { get; set; }
  public List<AddOn> AddOns { get; set; } = new();
  public bool IsGroupVisit { get; set; }
  public string? CustomText { get; set; }
  public int GroupSize { get; set; }
}
