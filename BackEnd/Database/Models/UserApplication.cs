using System.ComponentModel.DataAnnotations;

namespace BackEnd.Database.Models;

//[AuditInclude]
public sealed class UserApplication : IAuditable
{
    public int? UserId { get; set; }

    [MaxLength(20)]
    public string? AppId { get; set; }

    public int? RoleId { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsLogin { get; set; }

    public DateTime? LoginDate { get; set; }

    //[MaxLength(50)]
    //public string EntryBy { get; set; } = null!;

    //public DateTime? EntryDate { get; set; }

    //[MaxLength(50)]
    //public string UpdateBy { get; set; } = null!;

    //public DateTime? UpdateDate { get; set; }

    public bool IsDelete { get; set; }

    //[AuditIgnore]
    public AppUser? User { get; set; }

    //[AuditIgnore]
    public Application? Application { get; set; }

    //[AuditIgnore]
    public AppRole? AppRole { get; set; }

}