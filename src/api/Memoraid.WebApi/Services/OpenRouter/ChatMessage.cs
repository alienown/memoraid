namespace Memoraid.WebApi.Services.OpenRouter;

public class ChatMessage
{
    public required ChatRole Role { get; init; }
    public required string Content { get; init; }
}
