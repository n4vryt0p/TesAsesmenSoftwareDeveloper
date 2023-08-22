namespace BackEnd.Database.Models;

public class GroupMenuManagement
{
    public int? RoleId { get; set; }
    public string? AppId { get; set; }
    public int MenuId { get; set; }
    public string? MenuName { get; set; }
    public string? UniqueName { get; set; }
    public int ParentMenuId { get; set; }
    public string? ParentMenuName { get; set; }
    public string? MenuLink { get; set; }
    public int MenuIndex { get; set; }
    public bool IsRead { get; set; }
    public bool IsAdd { get; set; }
    public bool IsEdit { get; set; }
    public bool IsDelete { get; set; }
    public bool IsActive { get; set; }
    //public string? EntryBy { get; set; }
    //public DateTime EntryDate { get; set; }
    //public string? UpdateBy { get; set; }
    //public DateTime UpdateDate { get; set; }
}