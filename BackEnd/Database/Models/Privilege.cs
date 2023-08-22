namespace BackEnd.Database.Models;

public class Privilege
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public int MenuId { get; set; }
    public int MenuIndex { get; set; }
    public string MenuName { get; set; } = null!;
    public int? ParentMenuId { get; set; }
    public string MenuLink { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public string AppName { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string UniqueName { get; set; } = null!;
    public bool IsRead { get; set; }
    public bool IsAdd { get; set; }
    public bool IsEdit { get; set; }
    public bool IsDelete { get; set; }
    public bool IsActive { get; set; }
}