namespace FrontEnd.Services;

public interface IApiProducts
{
    ApiProduct Ready();
}

public class ApiProducts : IApiProducts
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _iConfig;

    public ApiProducts(IHttpClientFactory httpClientFactory, IConfiguration iConfig)
    {
        _httpClientFactory = httpClientFactory;
        _iConfig = iConfig;
    }

    public ApiProduct Ready()
    {
        var baseurl = _iConfig.GetSection("Configs")["BackEndApi"];
        var httpClient = _httpClientFactory.CreateClient("BaseClient");
        return new ApiProduct(baseurl, httpClient);
    }
}