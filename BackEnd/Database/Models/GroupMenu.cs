using System.ComponentModel.DataAnnotations;

namespace BackEnd.Database.Models;

//[AuditInclude]
public sealed class GroupMenu : IAuditable
{
    [Key, Required]
    public int RoleId { get; set; }

    [Key, Required]
    public int MenuId { get; set; }

    [Required]
    public bool IsRead { get; set; }

    [Required]
    public bool IsAdd { get; set; }

    [Required]
    public bool IsEdit { get; set; }

    [Required]
    public bool IsDelete { get; set; }

    //[Required]
    //public string EntryBy { get; set; } = null!;

    //[Required]
    //public DateTime EntryDate { get; set; }

    //[Required]
    //public string UpdateBy { get; set; } = null!;

    //[Required]
    //public DateTime UpdateDate { get; set; }

    //[AuditIgnore]
    public AppRole? AppRole { get; set; }

    //[AuditIgnore]
    public Menu? Menu { get; set; }
}