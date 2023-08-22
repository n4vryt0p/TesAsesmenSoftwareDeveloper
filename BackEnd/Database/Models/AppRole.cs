using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Database.Models;

public sealed class AppRole : IdentityRole<int>
{
    [Required]
    public bool IsDelete { get; set; }

    //[AuditIgnore]
    public List<GroupMenu>? GroupMenus { get; set; }
    //[AuditIgnore]
    public List<UserApplication>? UserApplications { get; set; }
}