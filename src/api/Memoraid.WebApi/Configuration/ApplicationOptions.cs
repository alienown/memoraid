namespace Memoraid.WebApi.Configuration;

public class ApplicationOptions
{
    public required OpenRouter OpenRouter { get; set; }
    public required Jwt Jwt { get; set; }
}

public class OpenRouter
{
    public required string ApiKey { get; set; }
    public required string ApiBaseUrl { get; set; }
}

public class Jwt
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Secret { get; set; }
}