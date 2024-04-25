using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Entities;

public class TelegramConnection
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid TelegramConnectionId { get; set; }
    public required string Tag { get; set; }
    public required string UserId { get; set; }
}
