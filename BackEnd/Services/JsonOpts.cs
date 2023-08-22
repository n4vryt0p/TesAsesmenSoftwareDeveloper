using System.Text.Json;

namespace BackEnd.Services;

public interface IJsonOptions
{
    JsonSerializerOptions JOpts();
}
public class JsonOpts : IJsonOptions
{
    // set this up how you need to!
    private readonly JsonSerializerOptions _jOpts = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        WriteIndented = false,
    };

    public JsonSerializerOptions JOpts()
    {
        return _jOpts;
    }
}