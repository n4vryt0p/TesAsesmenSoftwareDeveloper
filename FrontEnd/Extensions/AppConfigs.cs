using WebMarkupMin.AspNetCore7;

namespace FrontEnd.Extensions;

public static class AppConfigs
{
    public static WebApplication AppConfigurations(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }
        _ = app.UseSecurityHeaders(SecurityHeadersDefinitions.GetHeaderPolicyCollection(false));
        _ = app.UseForwardedHeaders();
        _ = app.UseResponseCaching();
        _ = app.UseResponseCompression();
        //app.UseHttpsRedirection();
        _ = app.UseStaticFiles();
        _ = app.UseCookiePolicy();
        _ = app.UseRouting();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        _ = app.UseWebMarkupMin();

        _ = app.MapRazorPages();

        return app;
    }
}