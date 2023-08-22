using System.Security.Claims;
using BackEnd.Database;
using BackEnd.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services;

public static class Auths
{
    public static WebApplication Authentications(this WebApplication app)
    {
        #region MapAPI

        _ = app.MapPost("/auth/login", async (UserPass loginx, DataContext db, IConfiguration config,
                UserManager<AppUser> userManage, RoleManager<AppRole> roleManage) =>
            {
                var appUser = await db.AppUsers.Include(rr => rr.Children).AsNoTracking().FirstOrDefaultAsync(au => au.UserName == loginx.Uname);

                var returnTrue = new AuthViewModel(null, null, null);
                if (appUser == null) return returnTrue;
                var userExist = await userManage.CheckPasswordAsync(appUser, loginx.Pass);
                if (!userExist) return returnTrue;
                
                var userRol = await userManage.GetRolesAsync(appUser);
                var privUsers = new List<PrivilegeVm>();
                if (userRol.Count > 0 && !userRol.Contains("SuperAdmin"))
                {
                    var userRoleIds = await roleManage.Roles.AsNoTracking().Where(x => userRol.Contains(x.Name))
                        .Select(x => x.Id).ToListAsync();
                    if (userRoleIds.Any())
                    {
                        privUsers = await db.GroupMenus
                            .AsNoTracking()
                            .Where(x => x.IsRead && x.Menu.IsActive && userRoleIds.Contains(x.RoleId) &&
                                        x.Menu.ParentMenuId == null)
                            .Select(x => new PrivilegeVm
                            {
                                AppId = x.Menu.AppId,
                                UniqueName = x.Menu.UniqueName,
                                Children = x.Menu.ChildMenu.Where(r =>
                                        r.GroupMenus.Any(d => userRoleIds.Contains(d.RoleId) && d.IsRead) && r.IsActive)
                                    .Select(g => new MenuDto2
                                    {
                                        UniqueName = g.UniqueName,
                                        AppId = g.AppId
                                    }).ToList()
                            })
                            .ToListAsync();
                    }
                }
                return new AuthViewModel(
                    new UserDto(
                        appUser.Id,
                        appUser.UserName ,
                        null,
                        appUser.FullName,
                        appUser.Email,
                        appUser.IsActive,
                        null
                        ),
                    userRol,
                    privUsers

                );
            })
            .WithTags("Authenticate")
            .WithName("GetAuthLogin")
            .AllowAnonymous();

        _ = app.MapPost("/auth/register", async (UserPass loginx,
                UserManager<AppUser> userManage) =>
        {
            var user = new AppUser { UserName = loginx.Uname, Email = loginx.Email, IsActive = true };
            var result = await userManage.CreateAsync(user, loginx.Pass);
            if (result.Succeeded)
            {
                var emailConfirm = await userManage.GenerateEmailConfirmationTokenAsync(user);
                if (!string.IsNullOrEmpty(emailConfirm))
                    await userManage.ConfirmEmailAsync(user, emailConfirm);
                if (loginx.Uname == "admin")
                    await userManage.AddToRoleAsync(user, "SuperAdmin");
                else
                    await userManage.AddToRoleAsync(user, "Kasir");
            }

            return result.Succeeded ? Results.Ok() : Results.BadRequest();
        })
            .WithTags("Authenticate")
            .WithName("GetAuthRegister")
            .AllowAnonymous();

        //Get menuList by AppId
        _ = app.MapGet("/MenulistByAppId", async (DataContext db, ClaimsPrincipal user) =>
                {
                    var gMenus = new List<MenuDto2>();
                    //string menuNumber = menus == "MenuList" ? "BOOK" : "BOOK2";
                    if (user.IsInRole("SuperAdmin"))
                    {
                        gMenus = await db.Menus.AsNoTracking()
                            .Where(e => e.IsActive && e.ParentMenuId == null)
                            .Select(t => new MenuDto2
                            {
                                MenuId = t.MenuId,
                                MenuName = t.MenuName,
                                ParentMenuId = t.ParentMenuId,
                                UniqueName = t.UniqueName,
                                MenuLink = t.MenuLink,
                                MenuIndex = t.MenuIndex,
                                Icon = t.Icon,
                                Children = t.ChildMenu.Where(r => r.IsActive).Select(g => new MenuDto2
                                {
                                    MenuId = g.MenuId,
                                    MenuName = g.MenuName,
                                    ParentMenuId = g.ParentMenuId,
                                    UniqueName = g.UniqueName,
                                    MenuLink = g.MenuLink,
                                    MenuIndex = g.MenuIndex,
                                    Icon = g.Icon,
                                    Children = g.ChildMenu.Where(r => r.IsActive).Select(gx => new MenuDto2
                                    {
                                        AppId = gx.AppId,
                                        MenuId = gx.MenuId,
                                        MenuName = gx.MenuName,
                                        ParentMenuId = gx.ParentMenuId,
                                        UniqueName = gx.UniqueName,
                                        MenuLink = gx.MenuLink,
                                        MenuIndex = gx.MenuIndex,
                                        Icon = gx.Icon,
                                    }).OrderBy(o => o.MenuIndex).ToList()
                                }).OrderBy(ox => ox.MenuIndex).ToList()
                            }).OrderBy(o => o.MenuIndex).ToListAsync();
                    }
                    else
                    {
                        var uRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(y => y.Value).ToList();
                        if (uRoles.Count > 0)
                        {
                            var userRoleIds = await db.Roles.AsNoTracking().Where(x => uRoles.Contains(x.Name))
                                .Select(y => y.Id).ToListAsync();
                            gMenus = await db.GroupMenus.AsNoTracking()
                                .Where(x => x.IsRead && x.Menu.IsActive && userRoleIds.Contains(x.RoleId) &&
                                            x.Menu.ParentMenuId == null)
                                .Select(t => new MenuDto2
                                {
                                    MenuId = t.Menu.MenuId,
                                    MenuName = t.Menu.MenuName,
                                    ParentMenuId = t.Menu.ParentMenuId,
                                    UniqueName = t.Menu.UniqueName,
                                    MenuLink = t.Menu.MenuLink,
                                    MenuIndex = t.Menu.MenuIndex,
                                    Icon = t.Menu.Icon,
                                    Children = t.Menu.ChildMenu.Where(r =>
                                            r.GroupMenus.Any(d => userRoleIds.Contains(d.RoleId) && d.IsRead) &&
                                            r.IsActive)
                                        .Select(g => new MenuDto2
                                        {
                                            MenuId = g.MenuId,
                                            MenuName = g.MenuName,
                                            ParentMenuId = g.ParentMenuId,
                                            UniqueName = g.UniqueName,
                                            MenuLink = g.MenuLink,
                                            MenuIndex = g.MenuIndex,
                                            Icon = g.Icon,
                                            Children = g.ChildMenu.Where(r => r.IsActive).Select(gx => new MenuDto2
                                            {
                                                MenuId = gx.MenuId,
                                                MenuName = gx.MenuName,
                                                ParentMenuId = gx.ParentMenuId,
                                                UniqueName = gx.UniqueName,
                                                MenuLink = gx.MenuLink,
                                                MenuIndex = gx.MenuIndex,
                                                Icon = gx.Icon,
                                            }).OrderBy(o => o.MenuIndex).ToList()
                                        }).OrderBy(ox => ox.MenuIndex).ToList()
                                }).OrderBy(o => o.MenuIndex).ToListAsync();
                        }
                    }

                    return gMenus;
                }
            )
            .WithTags("Menus")
            .WithName("MenulistByAppId")
            .RequireAuthorization();

        #endregion
        return app;
    }
}

#region Records
internal record UserPass(string? Uname, string? Pass, string? Email);

internal record AuthViewModel(UserDto? AppUser, IEnumerable<string>? AppRoles, IEnumerable<PrivilegeVm>? Privileges);

public sealed class PrivilegeVm
{
    public int MenuId { get; set; }
    public int MenuIndex { get; set; }
    public int? ParentMenuId { get; set; }
    public string MenuLink { get; set; } = null!;
    public string MenuName { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string UniqueName { get; set; } = null!;
    public bool IsRead { get; set; }
    public bool IsAdd { get; set; }
    public bool IsEdit { get; set; }
    public bool IsDelete { get; set; }
    public string Icon { get; set; } = null!;
    public int? Kolom { get; set; }
    public IEnumerable<MenuDto2>? Children { get; set; }
}

public sealed class MenuDto2
{
    public int? MenuId { get; set; }
    public int? ParentMenuId { get; set; }
    public string? AppId { get; set; }
    public string? AppName { get; set; }
    public string? MenuName { get; set; }
    public string? ParentMenuName { get; set; }
    public string? UniqueName { get; set; }
    public string? MenuLink { get; set; }
    public string? Icon { get; set; }
    public int? MenuIndex { get; set; }
    public bool? IsActive { get; set; }
    public int? Kolom { get; set; }
    public IEnumerable<MenuDto2>? Children { get; set; }
}
#endregion