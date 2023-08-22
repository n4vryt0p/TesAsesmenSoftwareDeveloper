using System.Text.Json;
using DevExtreme.AspNet.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.AdminConsole
{
    [Authorize]
    public class MenuManageModel : PageModel
    {
        private readonly IApiProducts _manageEngineApi;
        private readonly IJsonOptions _jOpt;

        public MenuManageModel(IApiProducts manageEngineApi, IJsonOptions jOpt)
        {
            _manageEngineApi = manageEngineApi;
            _jOpt = jOpt;
        }

        [BindProperty]
        public ICollection<GroupDdl>? GroupDdl { get; set; }

        public async Task OnGetAsync()
        {
            GroupDdl = await _manageEngineApi.Ready().MenusddlbyparentidAsync();
        }

        public async Task<IActionResult> OnGetReadAsync(DataSourceLoadOptionsBase set)
        {
            var response = await _manageEngineApi.Ready().GetMenusAsync();

            return new JsonResult(DataSourceLoader.Load(response, set));
        }

        public async Task OnPostCreateAsync(IFormCollection collection)
        {
            string? grup = collection["values"].FirstOrDefault();
            if (!string.IsNullOrEmpty(grup))
            {
                var rdto = JsonSerializer.Deserialize<Menu>(grup, _jOpt.JOpts());

                await _manageEngineApi.Ready().AddMenuAsync(rdto);
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

            await _manageEngineApi.Ready().UpdateMenuAsync(Convert.ToInt32(grupId), grup);
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

                await _manageEngineApi.Ready().DeleteMenuAsync(Convert.ToInt32(grupId));
                return null;
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }
    }
}
