using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using FrontEnd.Services;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FrontEnd.Extensions;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDiServices(this IServiceCollection services, ConfigurationManager cfgs, string? pCert)
    {
        // Add services to the container.
        services.AddRazorPages();
        _ = services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = false;
            options.SerializerOptions.AllowTrailingCommas = true;
        });

        //distribute cache
        var redisString = cfgs.GetConnectionString("RedisConnection");
        if (!string.IsNullOrEmpty(redisString))
        {
            _ = services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisString;
                options.InstanceName = "FrontendManageEngine";
            });
            _ = services.AddDistributedMemoryCache();
        }
        else
        {
            _ = services.AddMemoryCache();
        }
        _ = services.AddResponseCaching();

        //jwt/auth settings
        IdentityModelEventSource.ShowPII = true;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.HttpOnly = HttpOnlyPolicy.Always;
            })
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                //sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/auth/oops";
                options.LogoutPath = "/auth/logout";
                options.LoginPath = "/auth/login";
                //options.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);

                //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.IsEssential = true;
                //options.Cookie.Name = ".AuthCooked";
                //options.SessionStore = new InMemoryTicketStore();
                //options.Cookie.Domain = ".lps.go.id";
                //options.Cookie.HttpOnly = true;
                //options.Cookie.Path = "/";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.Cookie.MaxAge = options.ExpireTimeSpan; // optional
                options.SlidingExpiration = true;
            });

        //httpclient
        _ = services.AddHttpContextAccessor().AddHttpClient("BaseClient", httpClient =>
        {
            httpClient.DefaultRequestVersion = new Version(3, 0);
        }).AddHttpMessageHandler<OperationHandler>();

        //compresion & etc
        _ = services.AddResponseCompression(options => { options.EnableForHttps = true; });
        _ = services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        // Add WebMarkupMin services.
        _ = services.AddWebMarkupMin(options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = false;
            options.AllowCompressionInDevelopmentEnvironment = false;
        })
            .AddHtmlMinification(options =>
            {
                options.ExcludedPages = new List<IUrlMatcher>
                {
                    new WildcardUrlMatcher("/minifiers/x*ml-minifier"),
                    new ExactUrlMatcher("/contact"),
                    new ExactUrlMatcher("/saml/login"),
                    new ExactUrlMatcher("/saml/logout"),
                    new ExactUrlMatcher("/saml/artifact"),
                    new ExactUrlMatcher("/login/challenge")
                };

                var settings = options.MinificationSettings;
                settings.RemoveRedundantAttributes = true;

                options.CssMinifierFactory = new NUglifyCssMinifierFactory();
                options.JsMinifierFactory = new NUglifyJsMinifierFactory();
            })
            .AddXhtmlMinification(options =>
            {
                options.IncludedPages = new List<IUrlMatcher>
                {
                    new WildcardUrlMatcher("/minifiers/x*ml-minifier"),
                    new ExactUrlMatcher("/contact")
                };

                var settings = options.MinificationSettings;
                settings.RemoveRedundantAttributes = true;

                options.CssMinifierFactory = new KristensenCssMinifierFactory();
                options.JsMinifierFactory = new CrockfordJsMinifierFactory();
            })
            .AddXmlMinification(options =>
            {
                var settings = options.MinificationSettings;
                settings.CollapseTagsWithoutContent = true;
            })
            .AddHttpCompression(options =>
            {
                options.CompressorFactories = new List<ICompressorFactory>
                {
                    new BuiltInBrotliCompressorFactory(new BuiltInBrotliCompressionSettings
                    {
                        Level = CompressionLevel.Fastest
                    }),
                    new DeflateCompressorFactory(new DeflateCompressionSettings
                    {
                        Level = CompressionLevel.Fastest
                    }),
                    new GZipCompressorFactory(new GZipCompressionSettings
                    {
                        Level = CompressionLevel.Fastest
                    })
                };
            });
            
        _ = services.AddTransient<OperationHandler>();
        _ = services.AddSingleton<IJsonOptions, IJsonOpts>();
        _ = services.AddSingleton<IAuthSsoKc, AuthSsoKc>();
        _ = services.AddSingleton<IApiProducts, ApiProducts>();
        services.TryAdd(ServiceDescriptor.Transient<ITicketStore, InMemoryTicketStore>());
        _ = services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, ConfigureCookieAuthenticationOptions>();

        return services;
    }
}

//[JsonSerializable(typeof(ContohSaja[]))]
//internal partial class AppJsonSerializerContext : JsonSerializerContext
//{

//}