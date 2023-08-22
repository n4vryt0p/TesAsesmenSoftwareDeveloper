using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Database.Models;

//[AuditInclude]
public sealed class Menu : IAuditable
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MenuId { get; set; }

    [MaxLength(20)]
    public string? AppId { get; set; }

    public int? ParentMenuId { get; set; }

    [Required, MaxLength(255)]
    public string MenuName { get; set; } = null!;

    [Required, MaxLength(50)]
    public string UniqueName { get; set; } = null!;

    [MaxLength(50)]
    public string? Icon { get; set; }

    [Required, MaxLength(2000)]
    public string MenuLink { get; set; } = null!;

    [Required]
    public int MenuIndex { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
    public int? Kolom { get; set; }

    //[AuditIgnore]
    public Menu? ParentMenu { get; set; }

    //[AuditIgnore]
    public IEnumerable<Menu>? ChildMenu { get; set; }

    //[AuditIgnore]
    public Application? Application { get; set; }

    //[AuditIgnore]
    public IEnumerable<GroupMenu>? GroupMenus { get; set; }
}