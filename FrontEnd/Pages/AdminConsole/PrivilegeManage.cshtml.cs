using System.Text.Json;
using DevExtreme.AspNet.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.AdminConsole
{
    [Authorize]
    public class PrivilegeManageModel : PageModel
    {
        private readonly IApiProducts _manageEngineApi;
        private readonly IJsonOptions _jOpt;

        public PrivilegeManageModel(IApiProducts manageEngineApi, IJsonOptions jOpt)
        {
            _manageEngineApi = manageEngineApi;
            _jOpt = jOpt;
        }

        [BindProperty]
        public GroupRoleDdl? GroupRoleDdl { get; set; }

        public async Task OnGetAsync()
        {
            GroupRoleDdl = await _manageEngineApi.Ready().GetMenusDdlAsync();
        }

        public async Task<IActionResult> OnGetReadAsync(DataSourceLoadOptionsBase set)
        {
            var response = await _manageEngineApi.Ready().GetPrivilegesAsync();

            return new JsonResult(DataSourceLoader.Load(response, set));
        }

        public async Task OnPostCreateAsync(IFormCollection collection)
        {
            string? grup = collection["values"].FirstOrDefault();
            if (!string.IsNullOrEmpty(grup))
            {
                var rdto = JsonSerializer.Deserialize<GroupMenu>(grup, _jOpt.JOpts());

                await _manageEngineApi.Ready().AddPrivilegesAsync(rdto);
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

            var rdto = JsonSerializer.Deserialize<PrivilegesDto>(grupId, _jOpt.JOpts());
            var rdto2 = JsonSerializer.Deserialize<PrivilegesDto>(grup, _jOpt.JOpts());

            await _manageEngineApi.Ready().UpdatePrivilegesAsync(rdto?.RoleId ?? 0, rdto?.MenuId ?? 0, rdto2);
            return null;
        }

        public async Task<IActionResult?> OnDeleteDeleteAsync(IFormCollection collection)
        {
            try
            {
                string? privId = collection["key"].FirstOrDefault();
                //string? priv = collection["values"].FirstOrDefault();
                if (privId == null)
                {
                    return BadRequest("Data Error");
                }

                PrivilegesDto? privModel = JsonSerializer.Deserialize<PrivilegesDto>(privId, _jOpt.JOpts());

                await _manageEngineApi.Ready().DeletePrivilegesAsync(privModel?.RoleId ?? 0, privModel?.MenuId ?? 0);
                return null;
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }
    }
}
