using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace FrontEnd.Services;

public interface IAuthSsoKc
{
    //Task<ClaimsIdentity?> AddClaims(string userName);
    Task<ClaimModel?> AddClaims(string usrName, string pwd);
}


public class AuthSsoKc : IAuthSsoKc
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _iConfig;

    public AuthSsoKc(IConfiguration iConfig, IHttpClientFactory httpClientFactory)
    {
        _iConfig = iConfig;
        _httpClientFactory = httpClientFactory;
    }

    private string GenerateJwtToken(List<Claim> calim)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_iConfig.GetSection("Configs")["JwtKey"] ?? string.Empty));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
        DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(_iConfig.GetSection("Configs")["JwtExpireDays"]));

        JwtSecurityToken token = new(
            issuer: _iConfig.GetSection("Configs")["JwtIssuer"],
            audience: _iConfig.GetSection("Configs")["WebHost"],
            claims: calim,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ClaimModel?> AddClaims(string userName, string pwd)
    {
        //if (principal.Identity == null) return null;

        List<Claim> claims = new();
        //string guidString = Guid.NewGuid().ToString();
        string guidString2 = Guid.NewGuid().ToString();
        string? userPic;
        string? jwtToken;

        var baseurl = _iConfig.GetSection("Configs")["BackEndApi"];
        var httpClient = _httpClientFactory.CreateClient("BaseClient");
        var go = new ApiProduct(baseurl, httpClient);

        var modelResult = await go.GetAuthLoginAsync(new UserPass { Uname = userName, Pass = pwd });

        if (modelResult.AppUser != null)
        {
            //await using Stream stream2 = await response.Content.ReadAsStreamAsync();
            //AuthViewModel? modelResult = await JsonSerializer.DeserializeAsync<AuthViewModel>(stream2, _iJsonOpts.JOpts());
            //if (!string.IsNullOrEmpty(modelResult?.AppUser.Nik))
            //{
            //    if (!isSso)
            //    {
            //if (repone.AppUser != null)
            //{
            //    string pics = Convert.ToBase64String(repone.AppUser.Pic);
            //    string? exte = repone.AppUser.PicExt?[1..];
            //    userPic = $"data:image/{exte};base64,{pics}";
            //}
            //else
            //{
            userPic = "data:image/png;base64,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";
            //}
            //    }

            //    string[]? array = modelResult?.AppUser.FullName.Split(' ');
            //    string initial = "";
            //    if (array != null) initial = array.Aggregate(initial, (current, name) => current + name[..1].ToUpper());

            claims.AddRange(new List<Claim>
                {
                    new(ClaimTypes.DenyOnlySid, guidString2),
                    new(ClaimTypes.NameIdentifier, modelResult.AppUser.Id.ToString()),
                    new(ClaimTypes.Name, modelResult.AppUser.UserName ?? string.Empty),
                    new(ClaimTypes.Email, modelResult.AppUser.Email ?? string.Empty)
                });

            if (modelResult?.AppRoles != null)
            {
                claims.AddRange(modelResult.AppRoles.Select(appRole => new Claim(ClaimTypes.Role, appRole)));
            }

            jwtToken = GenerateJwtToken(claims);
            claims.Add(new Claim(ClaimTypes.UserData, jwtToken));

            var isSuperadmin = claims.Any(t => t.Value == "SuperAdmin");
            if (isSuperadmin)
            {
                string[]? confs = _iConfig.GetSection("Configs:AuthScopes").Get<string[]>();
                string? appId = _iConfig.GetSection("Configs")["AppId"];
                if (confs != null && appId != null)
                {
                    foreach (string conf in confs)
                    {
                        claims.Add(new Claim(appId, conf));
                    }
                }
            }
            else if (modelResult?.Privileges != null)
            {
                foreach (var priv in modelResult.Privileges)
                {
                    claims.Add(new Claim(priv.AppId, $"read:{priv.UniqueName}"));

                    if (priv.Children != null)
                    {
                        claims.AddRange(priv.Children.Select(item => new Claim(item.AppId!, $"read:{item.UniqueName}")));
                    }
                }
            }
            //    claims.Add(new Claim(ClaimTypes.PrimarySid, guidString));
            //    claims.Add(new Claim("Initial", initial));
            //    claims.Add(new Claim("CultureUI", CultureInfo.CurrentCulture.Name));
            //    claims.Add(new Claim("GroupId", modelResult?.AppUser.GroupId.ToString() ?? string.Empty));

            //    //dev
            //    //claims.Add(new Claim("HubUrl", $"{_iConfig.GetSection("Configs")["HubUrl"]}"));
            //    //prod
            //    var hubUrl1 = _iConfig.GetSection("Configs")["HubUrl"];
            //    claims.Add(new Claim("HubUrl", $"{hubUrl1}"));
            //}
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        return new ClaimModel
        {
            Pic = userPic,
            Tok = jwtToken,
            claimIdent = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
        };
    }
}

public class ClaimModel
{
    //public string? pin { get; set; }
    public string? Pic { get; init; }
    public string? Tok { get; init; }
    public ClaimsIdentity claimIdent { get; init; }
}