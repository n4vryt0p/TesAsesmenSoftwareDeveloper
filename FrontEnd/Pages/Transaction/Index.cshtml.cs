using System.Text.Json;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Transaction;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IApiProducts _manageEngineApi;
    private readonly IJsonOptions _jOpt;

    public IndexModel(IApiProducts manageEngineApi, IJsonOptions jOpt)
    {
        _manageEngineApi = manageEngineApi;
        _jOpt = jOpt;
    }

    [BindProperty]
    public ICollection<GroupDdl>? GroupRoleDdl { get; set; }

    public async Task OnGet()
    {
        GroupRoleDdl = await _manageEngineApi.Ready().MasterListDdlAsync();
    }

    public async Task<IActionResult> OnGetReadAsync(GridServerSide set)
    {
        var response = await _manageEngineApi.Ready().TransactionListServerSideAsync(set);

        return new JsonResult(response);
    }

    public async Task OnGetReadDataAsync(GridServerSide set)
    {
        var id = Request.Query["id"];
        var response = await _manageEngineApi.Ready().TransactionListServerSideAsync(set);
    }

    public async Task<IActionResult?> OnPutEditAsync(IFormCollection collection)
    {

        string? grupId = collection["key"].FirstOrDefault();
        string? grup = collection["values"].FirstOrDefault();
        if (grup == null || grupId == null)
        {
            return BadRequest("Data Error");
        }

        //await _manageEngineApi.Ready().UpdateNewsAsync(Convert.ToInt32(grupId), grup);
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

            //await _manageEngineApi.Ready().DeleteNewsAsync(Convert.ToInt32(grupId));
            return null;
        }
        catch
        {
            return BadRequest("Server Error");
        }
    }
}