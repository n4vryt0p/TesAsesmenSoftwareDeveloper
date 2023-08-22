using System.ComponentModel.DataAnnotations;

namespace BackEnd.Database.Models;

public sealed class Application
{
    [Key, Required, MaxLength(20)]
    public string AppId { get; set; } = null!;

    [Required, MaxLength(50)]
    public string AppName { get; set; } = null!;

    [Required]
    public bool IsDelete { get; set; }

    //[AuditIgnore]
    public IEnumerable<UserApplication>? UserApplications { get; set; }

    //[AuditIgnore]
    public IEnumerable<Menu>? Menus { get; set; }
}