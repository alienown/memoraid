namespace Memoraid.WebApi.Configuration;

public class ApplicationOptions
{
    public required OpenRouterOptions OpenRouter { get; set; }
}

public class OpenRouterOptions
{
    public required string ApiKey { get; set; }
    public required string ApiBaseUrl { get; set; }
}
