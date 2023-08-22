using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Database.Models;

//[AuditInclude]
public sealed class NotifData
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NotifDataId { get; set; }
    [MaxLength(2000)]
    public string? P256dh { get; set; }
    [MaxLength(2000)]
    public string? Auth { get; set; }
    [MaxLength(2000)]
    public string? Url { get; set; }

    public int? AppUserId { get; set; }
    //[AuditIgnore]
    public AppUser? AppUser { get; set; }
}