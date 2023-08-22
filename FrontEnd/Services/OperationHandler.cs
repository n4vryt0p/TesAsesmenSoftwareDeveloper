using System.Security.Claims;

namespace FrontEnd.Services;

public class OperationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public OperationHandler(IHttpContextAccessor contextAcc)
    {
        _contextAccessor = contextAcc;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? bearerToken = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.UserData);
        if (bearerToken != null && !request.Headers.Contains("Authorization"))
        {
            request.Headers.Add("Authorization", $"Bearer {bearerToken}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}