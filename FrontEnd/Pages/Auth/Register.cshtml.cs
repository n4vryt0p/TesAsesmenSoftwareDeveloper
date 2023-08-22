using AutoMapper;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Auth;

[ValidateAntiForgeryToken]
public class RegisterModel : PageModel
{
    private readonly IApiProducts _sportEventApi;

    public RegisterModel(IApiProducts sportEventApi)
    {
        _sportEventApi = sportEventApi;
    }

    [BindProperty]
    public UserPass? Regist { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostRegAsync(IFormCollection ddd)
    {
        var configs = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IFormCollection, UserPass>();
        });
        var mappers = new Mapper(configs);
        //var Regist = new CreateUser();
        mappers.Map(ddd, Regist);

        await _sportEventApi.Ready().GetAuthRegisterAsync(Regist);
        return new JsonResult("mappers");
    }

}