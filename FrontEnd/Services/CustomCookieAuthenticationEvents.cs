//using System.Security.Claims;
//using System.Text.Json;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Task = System.Threading.Tasks.Task;

//namespace FrontEnd.Services;

//public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
//{
//    private readonly IGetUserLastLogin _userLastLogin;

//    public CustomCookieAuthenticationEvents(IGetUserLastLogin userLastLogin)
//    {
//        _userLastLogin = userLastLogin;
//    }

//    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
//    {
//        var principal = context.Principal;
//        //// Look for the Id claim.
//        if (principal != null)
//        {
//            // Look for the LastChanged claim.
//            var lastChanged = principal.FindFirstValue("LastChanged");

//            if (!string.IsNullOrEmpty(lastChanged))
//            {
//                try
//                {
//                    var par = JsonSerializer.Deserialize<Dictionary<string, string>>(lastChanged);
//                    if (par != null)
//                    {
//                        var isChanged = await _userLastLogin.ValidateLastChangedAsync(par);
//                        if (!isChanged)
//                        {
//                            context.RejectPrincipal();

//                            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//                            await context.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
//                        }
//                    }
//                }
//                catch
//                {
//                    context.RejectPrincipal();

//                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//                    await context.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
//                }
//            }
//        }
//    }

//    //private object GenerateJwtToken(List<Claim> claims)
//    //{
//    //    SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_iConfig.GetSection("Configs")["JwtKey"]));
//    //    SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
//    //    DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(_iConfig.GetSection("Configs")["JwtExpireDays"]));

//    //    JwtSecurityToken token = new(
//    //        _iConfig.GetSection("Configs")["JwtIssuer"],
//    //        _iConfig.GetSection("Configs")["WebHost"],
//    //        claims,
//    //        expires: expires,
//    //        signingCredentials: creds
//    //    );

//    //    return new JwtSecurityTokenHandler().WriteToken(token);
//    //}
//}