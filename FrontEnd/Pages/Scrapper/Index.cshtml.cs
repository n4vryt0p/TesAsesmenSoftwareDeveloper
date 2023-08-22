using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Scrapper;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _iConfig;

    public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration iConfig)
    {
        _httpClientFactory = httpClientFactory;
        _iConfig = iConfig;
    }

    public void OnGet()
    {

    }
}