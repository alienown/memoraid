namespace Memoraid.WebApi.Configuration;

public class ApplicationOptions
{
    public required OpenRouter OpenRouter { get; set; }
}

public class OpenRouter
{
    public required string ApiKey { get; set; }
    public required string ApiBaseUrl { get; set; }
}