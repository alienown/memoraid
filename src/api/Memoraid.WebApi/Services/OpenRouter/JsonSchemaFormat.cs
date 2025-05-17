using System.Text.Json;

namespace Memoraid.WebApi.Services.OpenRouter;

public class JsonSchemaFormat
{
    public required string Name { get; init; }
    public required JsonElement Schema { get; init; }
}
