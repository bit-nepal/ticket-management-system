using System.ComponentModel.DataAnnotations.Schema;

namespace tms.Data;
public class Ticket
{
  [Column("id")]
  public int Id { get; set; }

  [Column("ticket_no")]
  public string EntryNo { get; set; } = null!;

  [Column("description")]
  public string Description { get; set; } = null!;

  [Column("time_period")]
  public string? TimePeriod { get; set; }
}
