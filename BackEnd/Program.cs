using BackEnd.Database;
using BackEnd.Database.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using BackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<DataContext>((sp, options) =>
    {
        _ = options.UseSqlServer(connectionString, b =>
        {
            _ = b.CommandTimeout(3600);
            _ = b.EnableRetryOnFailure(2);
            _ = b.MigrationsAssembly("BackEnd");
        });
    })
    .AddIdentity<AppUser, AppRole>(options =>
    {
        // Password settings
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Lockout.MaxFailedAccessAttempts = 3;
        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
        // User settings
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "ManageEngine API", Version = "v1" });
        option.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT token",
            Type = SecuritySchemeType.Http,
            Name = "Authorization",
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
    });

builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("admins_only", policy =>
        policy.RequireRole("SuperAdmin"));

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
var jwtKey = builder.Configuration.GetSection("Configs")["JwtKey"];
if (jwtKey != null)
{
    var keyx = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    builder.Services
        .AddAuthentication(x =>
        {
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = keyx,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            };
        });
}

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = false;
    options.SerializerOptions.AllowTrailingCommas = true;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddResponseCompression();

builder.Services.AddSingleton<IJsonOptions, JsonOpts>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
_ = app.UseForwardedHeaders();
_ = app
    .UseAuthentication()
    .UseAuthorization()
    .UseResponseCompression();

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.EnsureCreatedAsync();
}

app.UserMasterData();
app.Authentications();
app.Admins();
app.MasterData();

app.Run();

//namespace BackEnd
//{
//    internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//    {
//        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//    }
//}
