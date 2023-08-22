using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Database.Models;

//[AuditInclude]
public class Notification
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? ModuleName { get; set; }
    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? To { get; set; }
    public bool IsDirectToUrl { get; set; }

    public bool IsRead { get; set; }

    public string? Url { get; set; }

    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
}