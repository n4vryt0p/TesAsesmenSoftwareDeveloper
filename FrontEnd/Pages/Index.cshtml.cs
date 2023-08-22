using DevExtreme.AspNet.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages;

[Authorize]
public class IndexModel : PageModel
{
    //private readonly IApiProducts _manageEngineApi;

    //public IndexModel(IApiProducts manageEngineApi)
    //{
    //    _manageEngineApi = manageEngineApi;
    //}

    //[BindProperty]
    //public DashBoard? DashBoard { get; set; }
    //[BindProperty]
    //public DateRange? DateRange { get; set; }


    public void OnGetAsync()
    {
        //DashBoard = await _manageEngineApi.Ready().SumAsync(new DateRange{ SDate = new DateTime(2023,1,1), EDate = new DateTime(2023, 12, 1) });
    }

    //public async Task OnGetSyncNowAsync()
    //{
    //    //await _manageEngineApi.Ready().SyncNowAsync();
    //}

    ////public async Task<IActionResult> OnPostChangeDashAsync([FromBody] DateRange dr)
    ////{
    ////    var res = await _manageEngineApi.Ready().SumAsync(dr);
    ////    return new JsonResult(res);
    ////}

    //public async Task OnGetReadTopAsync(DataSourceLoadOptionsBase set)
    //{
    //    //var response = await _manageEngineApi.Ready().TopTenNewsAsync();

    //    //return new JsonResult(DataSourceLoader.Load(response, set));
    //}
}