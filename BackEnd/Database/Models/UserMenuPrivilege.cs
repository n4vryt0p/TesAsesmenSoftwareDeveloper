namespace BackEnd.Database.Models;

public class UserMenuPrivilege
{
    public int MenuId { get; set; }
    public string MenuName { get; set; } = null!;
    public int ParentMenuId { get; set; }
    public string MenuLink { get; set; } = null!;
}