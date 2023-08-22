using System.Text.Json;
using DevExtreme.AspNet.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Product;

public class IndexModel : PageModel
{
    private readonly IApiProducts _manageEngineApi;
    private readonly IJsonOptions _iConfig;

    public IndexModel(IJsonOptions iConfig, IApiProducts httpClientFactory)
    {
        _iConfig = iConfig;
        _manageEngineApi = httpClientFactory;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnGetReadAsync(DataSourceLoadOptionsBase set)
    {
        var response = await _manageEngineApi.Ready().GetMasterListServersideAsync();

        return new JsonResult(DataSourceLoader.Load(response, set));
    }

    public async Task OnPostCreateAsync(IFormCollection collection)
    {
        string? grup = collection["values"].FirstOrDefault();
        if (!string.IsNullOrEmpty(grup))
        {
            var rdto = JsonSerializer.Deserialize<FrontEnd.Product>(grup, _iConfig.JOpts());

            await _manageEngineApi.Ready().AddMastersAsync(rdto);
        }
    }

    public async Task<IActionResult?> OnPutEditAsync(IFormCollection collection)
    {

        string? grupId = collection["key"].FirstOrDefault();
        string? grup = collection["values"].FirstOrDefault();
        if (grup == null || grupId == null)
        {
            return BadRequest("Data Error");
        }

        var rdto = JsonSerializer.Deserialize<MasterDto>(grup, _iConfig.JOpts());

        await _manageEngineApi.Ready().UpdateMasterAsync(Convert.ToInt32(grupId), rdto);
        return null;
    }

    public async Task<IActionResult?> OnDeleteDeleteAsync(IFormCollection collection)
    {
        try
        {
            string? grupId = collection["key"].FirstOrDefault();
            if (grupId == null)
            {
                return BadRequest("Data Error");
            }

            await _manageEngineApi.Ready().DeleteMasterAsync(Convert.ToInt32(grupId));
            return null;
        }
        catch
        {
            return BadRequest("Server Error");
        }
    }
}