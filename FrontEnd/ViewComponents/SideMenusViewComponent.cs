using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.ViewComponents;

public class SideMenusViewComponent : ViewComponent
{

    private readonly IHttpContextAccessor _contAccessor;
    private readonly IApiProducts _manageEngineApi;

    public SideMenusViewComponent(IHttpContextAccessor contAccessor, IApiProducts manageEngineApi)
    {
        _contAccessor = contAccessor;
        _manageEngineApi = manageEngineApi;
    }

    public async Task<IViewComponentResult> InvokeAsync(string menu = "MenuHtml")
    {
        //string bearer = _contAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.UserData) ?? string.Empty;
        string? session = _contAccessor.HttpContext?.User.FindFirstValue(menu);
        bool isChanged = _contAccessor.HttpContext?.User.FindFirstValue("CultureUI") != CultureInfo.CurrentUICulture.Name;
        if (session == null || isChanged)
        {
            string menuNumber = menu == "MenuHtml" ? "MenuList" : "MenuList2";
            session = await GetMenuString(menuNumber);
            ClaimsIdentity claimsIdentity = new();
            claimsIdentity.AddClaim(new Claim(menu, session));
            _contAccessor?.HttpContext?.User.AddIdentity(claimsIdentity);
            if (_contAccessor?.HttpContext is not null)
            {
                await _contAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    _contAccessor.HttpContext.User);
            }
        }
        return View("default", session);
    }
    public async Task<string> GetMenuString(string menuList)
    {
        string? obj = _contAccessor.HttpContext?.User.FindFirstValue(menuList);
        ICollection<MenuDto2>? list;
        switch (obj)
        {
            case null:
                {
                    list = await GetAllMenu();
                    string meuJson = JsonSerializer.Serialize(list);
                    ClaimsIdentity claimsIdentity = new();
                    claimsIdentity.AddClaim(new Claim(menuList, meuJson));
                    _contAccessor.HttpContext?.User.AddIdentity(claimsIdentity);
                    if (_contAccessor.HttpContext is not null)
                    {
                        await _contAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, _contAccessor.HttpContext.User);
                    }

                    break;
                }

            default:
                list = JsonSerializer.Deserialize<ICollection<MenuDto2>>(obj);
                break;
        }
        return menuList == "MenuList" ? CreateMenu(list) : CreateMenu2(list);
    }

    public async Task<ICollection<MenuDto2>?> GetAllMenu() => await _manageEngineApi.Ready().MenulistByAppIdAsync();

    private string CreateMenu(ICollection<MenuDto2>? menus)
    {
        StringBuilder builder = new();
        if (menus == null)
        {
            return builder.ToString();
        }

        foreach (var parent in menus)
        {
            if (parent.ParentMenuId == null && parent.Children.Count < 1)
            {
                _ = builder.AppendLine($@"<li class=""nav-item"">
                                                    <a class=""nav-link menu-link"" href=""{parent.MenuLink}"">
                                                        <i class=""ri-{parent.Icon}""></i> <span data-key=""t-{parent.MenuName?.Replace(" ", "-").ToLower()}"">{parent.MenuName}</span>
                                                    </a>
                                                </li>");
            }
            else if (parent.ParentMenuId == null && parent.Children.Count > 0)
            {
                _ = builder.AppendLine($@"<li class=""nav-item"">
                                                    <a class=""nav-link menu-link"" href=""#{parent.UniqueName}"" data-bs-toggle=""collapse"" role=""button""
                                                       aria-expanded=""false"" aria-controls=""{parent.UniqueName}"">
                                                        <i class=""ri-{parent.Icon}""></i> <span data-key=""t-{parent.MenuName?.Replace(" ", "-").ToLower()}"">{parent.MenuName ?? ""}</span>
                                                    </a>");

                _ = builder.AppendLine($@"<div class=""collapse menu-dropdown"" id=""{parent.UniqueName}"">
                                                    <ul class=""nav nav-sm flex-column"">");

                foreach (var child in parent.Children)
                {
                    if (child.Children != null)
                    {
                        if (child.Children.Count > 0)
                        {
                            _ = builder.AppendLine($@"<li class=""nav-item"">
                                                            <a href=""#{child.UniqueName}"" class=""nav-link"" data-bs-toggle=""collapse"" role=""button""
                                                               aria-expanded=""false"" aria-controls=""{child.UniqueName}"" data-key=""t-{child.MenuName?.Replace(" ", "").ToLower()}"">
                                                                {child.MenuName ?? ""}
                                                            </a>
                                                            <div class=""collapse menu-dropdown"" id=""{child.UniqueName}"">
                                                                <ul class=""nav nav-sm flex-column"">");

                            foreach (var child2 in child.Children)
                            {
                                if (child2.Children != null)
                                {
                                    if (child2.Children.Count > 0)
                                    {
                                        _ = builder.AppendLine($@"<li class=""nav-item"">
                                                            <a href=""#{child2.UniqueName}"" class=""nav-link"" data-bs-toggle=""collapse"" role=""button""
                                                               aria-expanded=""false"" aria-controls=""{child2.UniqueName}"" data-key=""t-{child2.MenuName?.Replace(" ", "").ToLower()}"">
                                                                {child2.MenuName ?? ""}
                                                            </a>
                                                            <div class=""collapse menu-dropdown"" id=""{child2.UniqueName}"">
                                                                <ul class=""nav nav-sm flex-column"">");

                                        foreach (var child3 in child2.Children)
                                        {
                                            if (child3 != null)
                                            {
                                                _ = builder.Append($@"<li class=""nav-item"">
                                                            <a href=""{child3.MenuLink}"" class=""nav-link"" data-key=""t-{child3.MenuName?.Replace(" ", "").ToLower()}""> {child3.MenuName ?? ""} </a>
                                                        </li>");
                                            }
                                        }
                                        _ = builder.Append("</ul></div></li>");
                                    }
                                    else
                                    {
                                        _ = builder.AppendLine($@"<li class=""nav-item"">
                                                            <a href=""{child2.MenuLink}"" class=""nav-link"" data-key=""t-{child2.MenuName?.Replace(" ", "").ToLower()}""> {child2.MenuName ?? ""} </a>
                                                        </li>");
                                    }
                                }
                            }
                            _ = builder.Append("</ul></div></li>");
                        }
                        else
                        {
                            _ = builder.AppendLine($@"<li class=""nav-item"">
                                                            <a href=""{child.MenuLink}"" class=""nav-link"" data-key=""t-{child.MenuName?.Replace(" ", "").ToLower()}""> {child.MenuName ?? ""} </a>
                                                        </li>");
                        }
                    }
                }
                _ = builder.AppendLine("</ul></div></li>");
            }
        }
        return builder.ToString();
    }

    private string CreateMenu2(ICollection<MenuDto2>? menus)
    {
        if (menus?.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder builder = new();

        foreach (var parent in menus)
        {
            if (parent.Children.Count > 0)
            {
                string m = $@"<li class=""nav-item dropdown"">
                                            <a href=""#"" class=""nav-link dropdown-toggle"" data-toggle=""dropdown"">
                                                <i class=""ri-{parent?.Icon}""></i>
                                                {parent?.MenuName}
                                            </a>";
                _ = builder.AppendLine(m);

                int? isKolom = parent.Children.Max(y => y.Kolom);
                if (isKolom > 2)
                {

                    _ = builder.AppendLine($@"<div class=""dropdown-menu dropdown-menu-caret-center"" style=""width: 1000px; left: 1.5rem;""><div class=""row no-gutters"">");

                    for (int i = 1; i < 4; i++)
                    {
                        ICollection<MenuDto2>? childMenuss = parent?.Children.Where(t => t.Kolom == i).ToList();
                        if (childMenuss == null)
                        {
                            continue;
                        }

                        bool isBreak = i == isKolom;
                        _ = isBreak ? builder.Append($@"<div class=""col"">") : builder.Append($@"<div class=""col border-right"">");

                        foreach (var child in childMenuss)
                        {
                            if (child.Children?.Count > 0)
                            {
                                _ = builder.Append($@"<div class=""dropdown-header"">{child.MenuName}</div>");
                                foreach (var childern in child.Children)
                                {
                                    _ = builder.Append($@"<a class=""dropdown-item"" href=""{childern.MenuLink}"" target=""_blank"">{childern.MenuName}</a>");
                                }
                            }
                            else
                            {
                                _ = builder.Append($@"<a class=""dropdown-item"" href=""{child.MenuLink}"" target=""_blank"">{child.MenuName}</a>");
                            }
                        }

                        _ = builder.AppendLine("</div>");
                        if (isBreak)
                        {
                            break;
                        }
                    }
                    _ = builder.AppendLine("</div></div>");
                }
                else
                {
                    _ = builder.AppendLine($@"<div class=""dropdown-menu dropdown-menu-caret-center"" style=""max-width: 1000px; left: 1.5rem;""><div class=""row no-gutters"">");

                    for (int i = 1; i < 4; i++)
                    {
                        ICollection<MenuDto2>? childMenuss = parent?.Children.Where(t => t.Kolom == i).ToList();
                        if (childMenuss == null)
                        {
                            continue;
                        }

                        bool isBreak = i == isKolom;
                        _ = isBreak ? builder.Append($@"<div class=""col"">") : builder.Append($@"<div class=""col border-right"">");

                        foreach (var child in childMenuss)
                        {
                            if (child.Children?.Count > 0)
                            {
                                _ = builder.Append($@"<div class=""dropdown-header"">{child.MenuName}</div>");
                                foreach (var childern in child.Children)
                                {
                                    _ = builder.Append($@"<a class=""dropdown-item"" href=""{childern.MenuLink}"" target=""_blank"">{childern.MenuName}</a>");
                                }
                            }
                            else
                            {
                                _ = builder.Append($@"<a class=""dropdown-item"" href=""{child.MenuLink}"" target=""_blank"">{child.MenuName}</a>");
                            }
                        }

                        _ = builder.AppendLine("</div>");
                        if (isBreak)
                        {
                            break;
                        }
                    }
                    _ = builder.AppendLine("</div></div>");
                }
                _ = builder.AppendLine("</li>");

            }
        }


        return builder.ToString();
    }
}