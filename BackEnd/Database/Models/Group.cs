using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Database.Models;

//[AuditInclude]
public class Group
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }
    [MaxLength(1000)]
    public string NamaGrup { get; set; } = null!;
    [MaxLength(255)]
    public string Grup { get; set; } = null!;
    [MaxLength(255)]
    public string? ZoomId { get; set; }

    //[AuditIgnore]
    public ICollection<AppUser>? AppUsers { get; set; }
}