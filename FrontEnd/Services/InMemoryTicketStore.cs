using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;

namespace FrontEnd.Services;

public class InMemoryTicketStore : ITicketStore
{
    private const string KeyPrefix = "AuthSessionStore-";
    private readonly IMemoryCache _cache;

    public InMemoryTicketStore(IMemoryCache cache)
    {
        //_cache = new RedisCache(options);
        _cache = cache;
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var guid = Guid.NewGuid();

        var dopsid = ticket.Principal.FindFirstValue(ClaimTypes.DenyOnlyPrimarySid);
        if (!string.IsNullOrEmpty(dopsid))
        {
            var uG = ticket.Principal.Identity as ClaimsIdentity;
            var claimss = ticket.Principal.Claims.FirstOrDefault(t => t.Type == ClaimTypes.DenyOnlyPrimarySid);
            uG?.RemoveClaim(claimss);
        }

        ClaimsIdentity claimsIdentity = new();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.DenyOnlyPrimarySid, guid.ToString()));
        ticket.Principal.AddIdentity(claimsIdentity);
        AuthenticateResult.Success(ticket);

        var key = KeyPrefix + guid;
        await RenewAsync(key, ticket);
        return key;
    }

    public Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var options = new MemoryCacheEntryOptions();
        //var expiresUtc = ticket.Properties.ExpiresUtc;
        options.SetSlidingExpiration(TimeSpan.FromMinutes(15));
        options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        byte[] val = SerializeToBytes(ticket);
        _cache.Set(key, val, options);
        return Task.FromResult(0);
    }

    public Task<AuthenticationTicket?> RetrieveAsync(string key)
    {

        var bytes = (byte[]?)(_cache.Get(key) ?? null);
        var ticket = DeserializeFromBytes(bytes);
        return Task.FromResult(ticket);
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.FromResult(0);
    }

    private static byte[] SerializeToBytes(AuthenticationTicket source)
    {
        return TicketSerializer.Default.Serialize(source);
    }

    private static AuthenticationTicket? DeserializeFromBytes(byte[]? source)
    {
        return source == null ? null : TicketSerializer.Default.Deserialize(source);
    }
}