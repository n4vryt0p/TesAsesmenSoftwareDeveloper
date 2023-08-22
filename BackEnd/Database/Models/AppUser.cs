using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Database.Models;

public sealed class AppUser : IdentityUser<int>
{
    [MaxLength(255)]
    public string? Nik { get; set; }
    [Required]
    public bool IsLogin { get; set; }
    [Required]
    public bool IsActive { get; set; }
    [MaxLength(2000)]
    public string? RainbowId { get; set; }
    [MaxLength(511)]
    public string? FireBaseToken { get; set; }
    public bool? IsSuperAdmin { get; set; }
    [MaxLength(1000)]
    public string? FullName { get; set; }
    [MaxLength(2000)]
    public string? Keywords { get; set; }
    [MaxLength(2000)]
    public string? Tags { get; set; }

    //public int? GroupId { get; set; }

    public int? ParentId { get; set; }
    public AppUser? Parent { get; set; }
    public ICollection<AppUser> Children { get; } = new List<AppUser>();

    //[AuditIgnore]
    public ICollection<UserApplication> UserApplications { get; } = new List<UserApplication>();
    //[AuditIgnore]
    public ICollection<NotifData> NotifDatas { get; } = new List<NotifData>();
    //[AuditIgnore]
    public ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}