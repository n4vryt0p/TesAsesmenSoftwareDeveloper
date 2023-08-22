using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BackEnd.Database;
using BackEnd.Database.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services;

public static class UserMaster
{
    public static WebApplication UserMasterData(this WebApplication app)
    {
        #region UserLoginAndMenu
        
        _ = app.MapPost("/userlistaerverside", async (GridServerSide set, DataContext db, UserManager<AppUser> userManager) =>
        {
            string? filter = set.Filter;
            if (filter != null)
            {
                set.Filtering = new List<Filter>();
                List<object>? filterX = JsonSerializer.Deserialize<List<object>>(filter);
                string? tt = filterX?[1].ToString();
                if (tt != "and")
                {
                    string? col = filterX?[0].ToString();
                    if (col != null)
                        set.Filtering.Add(new Filter
                        {
                            Column = char.ToUpper(col[0]) + col[1..],
                            Value = filterX?[2].ToString()
                        });
                }
                else
                {
                    if (filterX != null)
                        foreach (object item in filterX)
                        {
                            JsonElement jsonElement = (JsonElement)item;
                            if (jsonElement.ValueKind == JsonValueKind.Array)
                            {
                                string col = jsonElement[0].ToString();
                                set.Filtering.Add(new Filter
                                {
                                    Column = char.ToUpper(col[0]) + col[1..],
                                    Value = jsonElement[2].ToString()
                                });
                            }
                        }
                }
            }
            var userDatas = db.AppUsers.Include(yy => yy.Children).IgnoreQueryFilters().AsNoTracking();

            var filtering = set.Filtering;
            if (filtering != null)
                userDatas = filtering.Aggregate(userDatas, (current, filt) => filt.Column switch
                {
                    "IsActive" => filt.Value == "True"
                        ? current.Where(t => t.IsActive)
                        : current.Where(t => !t.IsActive),
                    _ => current.Where(t => EF.Property<string>(t, filt.Column).Contains(filt.Value))
                });

            var totCount = userDatas.Count();

            var sorting = set.Sorting;
            if (sorting?.Length > 0)
                userDatas = userDatas.OrderBys(sorting[0].Selector, sorting[0].Desc);

            var dats = await userDatas.Skip(set.Skip ?? 0).Take(set.Take ?? 10).ToListAsync();

            var userDto = new List<UserDto>();
            foreach (var item in dats)
                userDto.Add(new UserDto
                (
                    item.Id,
                    item.UserName,
                    "******",
                    item.FullName,
                    item.Email,
                    item.IsActive,
                    await userManager.GetRolesAsync(item)
                ));
            return new { totalCount = totCount, data = userDto };
        }
            )
            .WithTags("Users")
            .WithName("GetUserListServersided")
            .RequireAuthorization("admins_only");

        //Add User
        _ = app.MapPost("/user/add", async (UserDto userdto, UserManager<AppUser> userManager) =>
            {
                var user = new AppUser { UserName = userdto.UserName, FullName = userdto.FullName, Email = userdto.Email, IsActive = true };
                if (userdto.Pass != null)
                {
                    var result = await userManager.CreateAsync(user, userdto.Pass);
                    if (result.Succeeded)
                    {

                        var emailConfirm = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        if (!string.IsNullOrEmpty(emailConfirm))
                            await userManager.ConfirmEmailAsync(user, emailConfirm);

                        if (userdto.Roles != null)
                        {
                            var result2 = await userManager.AddToRolesAsync(user, userdto.Roles);
                            if (!result2.Succeeded)
                                Results.BadRequest();
                        }
                    }

                    return result.Succeeded ? Results.Ok() : Results.BadRequest();
                }

                return Results.NotFound();
            })
            .WithTags("Users")
            .WithName("AddUser")
            .RequireAuthorization("admins_only");

        //Edit User
        _ = app.MapPut("/user/edit/{userId}", async (int userId, UserDto userdto, UserManager<AppUser> userManager, DataContext db) =>
                {
                    var userX = await userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
                    if (userX == null)
                        return Results.NotFound("No such user");

                    if (!string.IsNullOrEmpty(userdto.UserName)) userX.UserName = userdto.UserName;
                    if (!string.IsNullOrEmpty(userdto.FullName)) userX.FullName = userdto.FullName;
                    if (!string.IsNullOrEmpty(userdto.Email)) userX.Email = userdto.Email;

                    StringBuilder msg = new();
                    
                    if (userdto.Roles != null)
                    {
                        var existRoles = await userManager.GetRolesAsync(userX);
                        if (existRoles.Count > 0)
                        {
                            var result = await userManager.RemoveFromRolesAsync(userX, existRoles);
                            if (!result.Succeeded)
                                _ = msg.AppendLine("Eror when reset roles, please call helpdesk");

                        }
                        var result2 = await userManager.AddToRolesAsync(userX, userdto.Roles);
                        if (!result2.Succeeded)
                            _ = msg.AppendLine("Eror when adding roles, please call helpdesk.");
                    }

                    if (!string.IsNullOrEmpty(msg.ToString())) return Results.BadRequest(msg.ToString());

                    _ = db.AppUsers.Update(userX);
                    _ = await db.SaveChangesAsync();

                    return Results.Ok();
                })
                .WithTags("Users")
                .WithName("UpdateUser")
                .RequireAuthorization("admins_only");

        //Delete User
        _ = app.MapDelete("/user/delete/{userId}",
                async (int userId, UserManager<AppUser> userManager, DataContext db) =>
                {
                    var userX = await userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
                    if (userX == null)
                        return Results.NotFound("No such user");



                    _ = db.AppUsers.Remove(userX);
                    _ = await db.SaveChangesAsync();

                    return Results.Ok();
                })
            .WithTags("Users")
            .WithName("DeleteUser")
            .RequireAuthorization("admins_only");

        //Get User Roles & User to assigned
        _ = app.MapGet("/userandroles", async (DataContext db) =>
            {
                return new GroupRoleDdl
                {
                    Groups = await db.AppUsers.AsNoTracking()//.Where(tt => tt.Id != userId)
                        .Select(x => new GroupDdl { Id = x.Id, Text = x.UserName })
                        .ToListAsync(),
                    Roles = await db.AppRoles.AsNoTracking()
                        .Select(x => new GroupDdl
                        {
                            Id = x.Id,
                            Text = x.Name
                        }).ToListAsync()
                };
            })
            .WithTags("Users")
            .WithName("GetUserAndRoles")
            .RequireAuthorization();

        #endregion
        return app;
    }
}

#region Records/models

public class GridServerSide
{
    [JsonConstructor]
    public GridServerSide(int? skip, int? take, bool? requireTotalCount, string? sort, string? filter)
    {
        Skip = skip;
        Take = take;
        RequireTotalCount = requireTotalCount;
        Sort = sort;
        Filter = filter;
    }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public bool? RequireTotalCount { get; set; }
    public string? Sort { get; set; }
    public string? Filter { get; set; }
    public virtual SortingInfo[]? Sorting => !string.IsNullOrEmpty(Sort) ? JsonSerializer.Deserialize<List<SortingInfo>>(Sort, new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        WriteIndented = false
    })!.ToArray() : null;
    public virtual ICollection<Filter>? Filtering { get; set; }
}

//public class Sort
//{
//    public string? Selector { get; set; }
//    public bool? Desc { get; set; }
//}
public class Filter
{
    public string? Column { get; set; }
    public string? Value { get; set; }
    public int? Type { get; set; }
}

internal record UserDto(int Id, string? UserName, string? Pass, string? FullName, string? Email, bool IsActive, IEnumerable<string>? Roles)
{
}

#endregion

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBys<T>(this IQueryable<T> source, string? sortColumn, bool sortColumnDir)
    {
        Expression expression = source.Expression;
        sortColumn = string.Concat(sortColumn?.First().ToString(), sortColumn.AsSpan(1));
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        MemberExpression selector = Expression.PropertyOrField(parameter, sortColumn);
        string method = sortColumnDir ? "OrderByDescending" : "OrderBy";
        expression = Expression.Call(typeof(Queryable), method,
            new[] { source.ElementType, selector.Type },
            expression, Expression.Quote(Expression.Lambda(selector, parameter)));
        return source.Provider.CreateQuery<T>(expression);
    }
}