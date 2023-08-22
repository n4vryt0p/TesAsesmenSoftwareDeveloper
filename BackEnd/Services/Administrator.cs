using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Claims;
using BackEnd.Database;
using BackEnd.Database.Models;
using Newtonsoft.Json;

namespace BackEnd.Services;

public static class Administrator
{
    public static WebApplication Admins(this WebApplication app)
    {
        #region Roles

        //Get all roles
        _ = app.MapGet("/rolelist", async (DataContext db) =>
                await db.AppRoles.AsNoTracking()
                    .Select(x => new GroupDto
                    {
                        RoleId = x.Id,
                        RoleName = x.Name
                    }).ToListAsync()
            )
            .WithTags("Roles")
            .WithName("GetRoleList")
            .RequireAuthorization("admins_only");

        //Get all roles DDL
        _ = app.MapGet("/rolelistddl", async (DataContext db) =>
                await db.AppRoles.AsNoTracking()
                    .Select(x => new GroupDdl
                    {
                        Id = x.Id,
                        Text = x.Name
                    }).ToListAsync()
            )
            .WithTags("Roles")
            .WithName("GetRoleListDDL")
            .RequireAuthorization("admins_only");

        //Add role
        _ = app.MapPost("/role/add", async (RoleDto userdto, RoleManager<AppRole> roleManager) =>
            {

                _ = await roleManager.CreateAsync(new AppRole
                {
                    Name = userdto.RoleName,
                    IsDelete = false
                });

                return Results.Ok();
            })
            .WithTags("Roles")
            .WithName("AddRole")
            .RequireAuthorization("admins_only");

        //Edit role
        _ = app.MapPut("/role/edit/{roleId}", async (int roleId, RoleDto userdto, RoleManager<AppRole> roleManager) =>
            {
                var existRole = await roleManager.FindByIdAsync(roleId.ToString());
                if (existRole == null)
                    return Results.BadRequest("No such role");

                existRole.Name = userdto.RoleName;

                _ = await roleManager.UpdateAsync(existRole);

                return Results.Ok();
            })
            .WithTags("Roles")
            .WithName("UpdateRole")
            .RequireAuthorization("admins_only");

        //Delete role
        _ = app.MapDelete("/role/delete/{id}", async (int id, RoleManager<AppRole> roleManager) =>
            {
                var user = await roleManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return Results.BadRequest("No such role");

                _ = await roleManager.DeleteAsync(user);
                return Results.Ok();
            })
            .WithTags("Roles")
            .WithName("DeleteRole")
            .RequireAuthorization("admins_only");

        #endregion


        #region Privilege

        //Get all menus or by menuid
        _ = app.MapGet("/priv", async (DataContext db) =>
                await db.GroupMenus.AsNoTracking().Select(t => new Privilege
                {
                    RoleId = t.RoleId,
                    MenuId = t.MenuId,
                    UniqueName = t.Menu!.UniqueName,
                    MenuName = t.Menu.MenuName,
                    Icon = t.Menu.Icon!,
                    MenuLink = t.Menu.MenuLink,
                    MenuIndex = t.Menu.MenuIndex,
                    ParentMenuId = t.Menu.ParentMenuId,
                    IsRead = t.IsRead,
                    IsAdd = t.IsAdd,
                    IsEdit = t.IsEdit,
                    IsDelete = t.IsDelete,
                    IsActive = t.Menu.IsActive
                }).ToListAsync()
            )
            .WithTags("Privileges")
            .WithName("GetPrivileges")
            .RequireAuthorization("admins_only");

        //Get all menus ddl or by appid
        //app.MapGet("/privDdl", async (UserDbContext db) =>
        //        await db.Menus.AsNoTracking()
        //        .Select(x => new GroupDdl{ Id = x.MenuId, Text = $"{x.MenuName}: {x.MenuLink}" })
        //        .ToListAsync()
        //    )
        //    .WithTags("Privileges")
        //    .WithName("GetPrivilegesDdl")
        //    .IncludeInOpenApi();

        //Post app to add
        _ = app.MapPost("/priv/add", async (DataContext db, GroupMenu model, ClaimsPrincipal user) =>
        {
            db.Entry(model).Property("CreateBy").CurrentValue = user.Identity?.Name;

            _ = await db.GroupMenus.AddAsync(model);
            _ = await db.SaveChangesAsync();
            return Results.Ok();
        })
            .WithTags("Privileges")
            .WithName("AddPrivileges")
            .RequireAuthorization("admins_only");

        //Put app to edit
        _ = app.MapPut("/priv/edit/{roleId}/{menuId}", async (int roleId, int menuId, PrivilegesDto updatePriv, DataContext db, ClaimsPrincipal user) =>
        {
            var appmen = await db.GroupMenus.FirstOrDefaultAsync(t => t.MenuId == menuId && t.RoleId == roleId);
            if (appmen is null)
                return Results.NotFound();

            appmen.IsAdd = updatePriv.IsAdd ?? appmen.IsAdd;
            appmen.IsEdit = updatePriv.IsEdit ?? appmen.IsEdit;
            appmen.IsDelete = updatePriv.IsDelete ?? appmen.IsDelete;
            appmen.IsRead = updatePriv.IsRead ?? appmen.IsRead;
            appmen.RoleId = updatePriv.RoleId ?? appmen.RoleId;
            appmen.MenuId = updatePriv.MenuId ?? appmen.MenuId;


            db.Entry(appmen).Property("UpdateBy").CurrentValue = user.Identity?.Name;
            db.Entry(appmen).Property("UpdateDate").CurrentValue = DateTime.UtcNow;

            _ = db.GroupMenus.Update(appmen);
            _ = await db.SaveChangesAsync();
            return Results.Ok();

        })
            .WithTags("Privileges")
            .WithName("UpdatePrivileges")
            .RequireAuthorization("admins_only");

        //Delete app by id
        //app.MapDelete("/priv/delete/{menuId}", async (UserDbContext db, int menuId, ClaimsPrincipal user) =>
        //{
        //    var app = await db.Menus.FirstOrDefaultAsync(t => t.MenuId == menuId);
        //    if (app is null)
        //        return Results.NotFound();

        //    db.Menus.Remove(app);
        //    await db.SaveChangesAsync();
        //    return Results.Ok();
        //})
        //    .WithTags("Privileges")
        //    .WithName("DeletePrivileges")
        //    .IncludeInOpenApi();

        //Delete app by id
        _ = app.MapDelete("/priv/delete/{roleId}/{MenuId}", async (int roleId, int menuId, DataContext db, ClaimsPrincipal user) =>
        {
            var appmen = await db.GroupMenus.FirstOrDefaultAsync(t => t.MenuId == menuId && t.RoleId == roleId);
            if (appmen is null)
                return Results.NotFound();

            _ = db.GroupMenus.Remove(appmen);
            _ = await db.SaveChangesAsync();
            return Results.Ok();
        })
            .WithTags("Privileges")
            .WithName("DeletePrivileges")
            .RequireAuthorization("admins_only");

        #endregion

        #region Menu

        //Get all menus
        _ = app.MapGet("/menus", async (DataContext db) =>
                await db.Menus.AsNoTracking()
                            //.OrderBy(x => new { x.ParentMenuId, x.MenuIndex })
                            .Select(t => new MenuDto2
                            {
                                MenuId = t.MenuId,
                                AppId = t.Application!.AppId,
                                MenuName = t.MenuName,
                                ParentMenuId = t.ParentMenuId,
                                ParentMenuName = t.ParentMenu!.MenuName,
                                UniqueName = t.UniqueName,
                                MenuLink = t.MenuLink,
                                MenuIndex = t.MenuIndex,
                                Icon = t.Icon,
                                IsActive = t.IsActive,
                                Kolom = t.Kolom,
                            }).ToListAsync()
            )
            .WithTags("Menus")
            .WithName("GetMenus")
            .RequireAuthorization();

        //Get menu by menuid
        _ = app.MapGet("/menus/{menuId}", async (int menuId, DataContext db) =>
                await db.Menus.AsNoTracking().FirstOrDefaultAsync(y => y.MenuId == menuId)
            )
            .WithTags("Menus")
            .WithName("GetMenuById")
            .RequireAuthorization();

        //Get all menus ddl or by appid
        _ = app.MapGet("/menusddl", async (DataContext db) =>
        {
            return new GroupRoleDdl
            {
                Groups = await db.Menus.AsNoTracking()
                    .Select(x => new GroupDdl { Id = x.MenuId, Text = $"{x.MenuName}: {x.MenuLink}" })
                    .ToListAsync(),
                Roles = await db.AppRoles.AsNoTracking()
                    .Select(x => new GroupDdl
                    {
                        Id = x.Id,
                        Text = x.Name
                    }).ToListAsync(),
            };
        })
        .WithTags("Menus")
        .WithName("GetMenusDdl")
        .RequireAuthorization();

        //Get all menus ddl by parentid
        _ = app.MapGet("/menusddlbyparentid", async (DataContext db) =>
                await db.Menus.AsNoTracking().Where(y => y.MenuLink == "#" | y.MenuLink == "/" | y.MenuLink == "-")
                .Select(x => new GroupDdl { Id = x.MenuId, Text = $"{x.MenuName}: {x.MenuLink}", ZoomId = x.AppId })
                .ToListAsync()
            )
            .WithTags("Menus")
            .WithName("menusddlbyparentid")
            .RequireAuthorization();

        //Post app to add
        _ = app.MapPost("/menus/add", async (Menu model, DataContext db, ClaimsPrincipal user) =>
        {
            if (string.IsNullOrEmpty(model.AppId))
                model.AppId = "WebScrap";

            db.Entry(model).Property("CreateBy").CurrentValue = user.Identity?.Name;

            _ = await db.Menus.AddAsync(model);
            _ = await db.SaveChangesAsync();
            return Results.Ok();
        })
            .WithTags("Menus")
            .WithName("AddMenu")
            .RequireAuthorization("admins_only");

        //Put app to edit
        _ = app.MapPut("/menus/edit/{appId}", async (int appId, string menu, DataContext db, ClaimsPrincipal user) =>
            {
                var obj = await db.Menus.FirstOrDefaultAsync(t => t.MenuId == appId);
                if (obj != null)
                {
                    JsonConvert.PopulateObject(menu, obj);
                    db.Entry(obj).Property("UpdateBy").CurrentValue = user.Identity?.Name;
                    db.Entry(obj).Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                    _ = db.Menus.Update(obj);
                    _ = await db.SaveChangesAsync();
                }

                return Results.Ok();

        })
            .WithTags("Menus")
            .WithName("UpdateMenu")
            .RequireAuthorization("admins_only");

        //Delete app by id
        _ = app.MapDelete("/menus/delete/{appId}", async (int appId, DataContext db, ClaimsPrincipal user) =>
        {
            var appmen = await db.Menus.FirstOrDefaultAsync(t => t.MenuId == appId);
            if (appmen is null)
                return Results.NotFound();

            appmen.IsDeleted = true;
            db.Entry(appmen).Property("UpdateBy").CurrentValue = user.Identity?.Name;
            db.Entry(appmen).Property("UpdateDate").CurrentValue = DateTime.UtcNow;

            _ = db.Menus.Update(appmen);
            _ = await db.SaveChangesAsync();
            return Results.Ok();
        })
            .WithTags("Menus")
            .WithName("DeleteMenu")
            .RequireAuthorization("admins_only");

        //Get menuList by User
        _ = app.MapGet("/menulist", async (DataContext db, ClaimsPrincipal user, RoleManager<AppRole> roleManage) =>
        {
            var gMenus = new List<MenuDto2>();
            if (user.IsInRole("SuperAdmin"))
            {
                gMenus = await db.Menus.AsNoTracking()
                        .Where(e => e.IsActive && e.ParentMenuId == null)
                        .Select(t => new MenuDto2
                        {
                            AppId = t.AppId,
                            MenuId = t.MenuId,
                            MenuName = t.MenuName,
                            ParentMenuId = t.ParentMenuId,
                            MenuLink = t.MenuLink,
                            MenuIndex = t.MenuIndex,
                            Icon = t.Icon,
                            Kolom = t.Kolom,
                            Children = (t.ChildMenu ?? Array.Empty<Menu>()).Where(r => r.IsActive).Select(g => new MenuDto2
                            {
                                AppId = g.AppId,
                                MenuId = g.MenuId,
                                MenuName = g.MenuName,
                                ParentMenuId = g.ParentMenuId,
                                MenuLink = g.MenuLink,
                                MenuIndex = g.MenuIndex,
                                Icon = g.Icon,
                                Kolom = g.Kolom,
                                Children = (g.ChildMenu ?? Array.Empty<Menu>()).Where(r => r.IsActive).Select(gx => new MenuDto2
                                {
                                    AppId = gx.AppId,
                                    MenuId = gx.MenuId,
                                    MenuName = gx.MenuName,
                                    ParentMenuId = gx.ParentMenuId,
                                    MenuLink = gx.MenuLink,
                                    MenuIndex = gx.MenuIndex,
                                    Icon = gx.Icon,
                                    Kolom = gx.Kolom
                                }).OrderBy(o => o.MenuIndex).ToList()
                            }).OrderBy(o => o.MenuIndex).ToList()
                        }).OrderBy(x => x.MenuIndex).ToListAsync();
            }
            else
            {
                var uRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(y => y.Value).ToList();
                if (uRoles.Count > 0)
                {
                    var userRoleIds = await roleManage.Roles.AsNoTracking().Where(x => uRoles.Contains(x.Name)).Select(y => y.Id).ToListAsync();
                    gMenus = await db.GroupMenus.AsNoTracking()
                        .Where(e => userRoleIds.Contains(e.RoleId) && e.IsRead && e.Menu.IsActive && e.Menu.ParentMenuId == null)
                        .Select(t => new MenuDto2
                        {
                            AppId = t.Menu!.AppId,
                            MenuId = t.Menu.MenuId,
                            MenuName = t.Menu.MenuName,
                            ParentMenuId = t.Menu.ParentMenuId,
                            MenuLink = t.Menu.MenuLink,
                            MenuIndex = t.Menu.MenuIndex,
                            Icon = t.Menu.Icon,
                            Kolom = t.Menu.Kolom,
                            Children = (t.Menu.ChildMenu ?? Array.Empty<Menu>()).Where(r => r.GroupMenus != null && r.GroupMenus.Any(d => userRoleIds.Contains(d.RoleId) && d.IsRead) && r.IsActive).Select(g => new MenuDto2
                            {
                                AppId = g.AppId,
                                MenuId = g.MenuId,
                                MenuName = g.MenuName,
                                ParentMenuId = g.ParentMenuId,
                                MenuLink = g.MenuLink,
                                MenuIndex = g.MenuIndex,
                                Icon = g.Icon,
                                Kolom = g.Kolom,
                                Children = (g.ChildMenu ?? Array.Empty<Menu>()).Where(r => r.IsActive).Select(gx => new MenuDto2
                                {
                                    AppId = gx.AppId,
                                    MenuId = gx.MenuId,
                                    MenuName = gx.MenuName,
                                    ParentMenuId = gx.ParentMenuId,
                                    MenuLink = gx.MenuLink,
                                    MenuIndex = gx.MenuIndex,
                                    Icon = gx.Icon,
                                    Kolom = gx.Kolom
                                }).OrderBy(o => o.MenuIndex).ToList()
                            }).OrderBy(o => o.MenuIndex).ToList()
                        }).OrderBy(x => x.MenuIndex).ToListAsync();
                }
            }

            return gMenus;
        }
            )
            .WithTags("Menus")
            .WithName("GetMenuListByUser")
            .RequireAuthorization();

        #endregion

        return app;
    }
}
public class GroupDto
{
    [DisplayName("Role Id")]
    public int RoleId { get; set; }

    [DisplayName("Group Id")]
    public int GroupId { get; set; }

    [MaxLength(20)]
    [DisplayName("Application Name")]
    public string? AppId { get; set; }

    [MaxLength(1000)]
    [DisplayName("Group Name")]
    public string? GroupName { get; set; }

    [MaxLength(255)]
    [DisplayName("Group")]
    public string? Group { get; set; }

    [MaxLength(50)]
    [DisplayName("Role Name")]
    public string? RoleName { get; set; }

    [DisplayName("Is Delete")]
    public bool IsDelete { get; set; }

    [ForeignKey(nameof(AppId))]
    public virtual ApplicationDto? Application { get; set; }
    public string? ZoomId { get; set; }
}
public class GroupDdl
{
    [DisplayName("Group Id")]
    public int Id { get; set; }

    [MaxLength(50)]
    [DisplayName("Group Name")]
    public string? Text { get; set; }

    //[DisplayName("Zoom Id")]
    [MaxLength(50)]
    public string? ZoomId { get; set; }
}
public class ApplicationDto
{
    [MaxLength(20), Required]
    [DisplayName("Application Id")]
    public string AppId { get; set; } = null!;

    [MaxLength(50)]
    [DisplayName("Application Name")]
    public string AppName { get; set; } = null!;
    public bool? IsDelete { get; set; }
}
public class RoleDto
{
    public int? RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
}
public class PrivilegesDto
{
    public int? UserId { get; set; }
    public int? RoleId { get; set; }
    public int? MenuId { get; set; }
    public int? MenuIndex { get; set; }
    public string? MenuName { get; set; }
    public int? ParentMenuId { get; set; }
    public string? MenuLink { get; set; }
    public string? UserName { get; set; }
    public string? AppName { get; set; }
    public string? AppId { get; set; }
    public string? UniqueName { get; set; }
    public bool? IsRead { get; set; }
    public bool? IsAdd { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsDelete { get; set; }
    public bool? IsActive { get; set; }
}
public class GroupRoleDdl
{
    public IEnumerable<GroupDdl>? Groups { get; set; }
    public IEnumerable<GroupDdl>? Roles { get; set; }
    public IEnumerable<GroupDdl>? Package { get; set; }
}