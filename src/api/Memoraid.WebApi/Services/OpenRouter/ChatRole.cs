using System.Text.Json.Serialization;

namespace Memoraid.WebApi.Services.OpenRouter;

public enum ChatRole
{
    [JsonPropertyName("system")]
    System,

    [JsonPropertyName("user")]
    User,

    [JsonPropertyName("assistant")]
    Assistant
}
