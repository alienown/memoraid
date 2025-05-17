using System.Collections.Generic;

namespace Memoraid.WebApi.Services.OpenRouter;

public class CompleteWithStructuredOutputRequest
{
    public required string Model { get; init; }
    public required List<ChatMessage> Messages { get; init; }
    public required JsonSchemaFormat JsonSchema { get; init; }
}
