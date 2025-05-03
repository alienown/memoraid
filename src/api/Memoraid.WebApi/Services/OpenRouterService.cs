using Memoraid.WebApi.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public enum ChatRole
{
    [JsonPropertyName("system")]
    System,

    [JsonPropertyName("user")]
    User,

    [JsonPropertyName("assistant")]
    Assistant
}

public class ChatMessage
{
    public required ChatRole Role { get; init; }
    public required string Content { get; init; }
}

public class CompleteWithStructuredOutputRequest
{
    public required string Model { get; init; }
    public required List<ChatMessage> Messages { get; init; }
    public required JsonSchemaFormat JsonSchema { get; init; }
}

public class JsonSchemaFormat
{
    public required string Name { get; init; }
    public required JsonElement Schema { get; init; }
}

public interface IOpenRouterService
{
    Task<T> CompleteWithStructuredOutputAsync<T>(CompleteWithStructuredOutputRequest request);
}

internal class OpenRouterService : IOpenRouterService
{
    private const string CompletionsApiUrl = "/api/v1/chat/completions";

    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenRouterService(HttpClient httpClient, IOptions<ApplicationOptions> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.OpenRouter.ApiKey;
    }

    public async Task<T> CompleteWithStructuredOutputAsync<T>(CompleteWithStructuredOutputRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, CompletionsApiUrl)
        {
            Content = JsonContent.Create(new
            {
                model = request.Model,
                messages = request.Messages.Select(m => new
                {
                    role = m.Role.ToString().ToLower(),
                    content = m.Content
                }),
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = request.JsonSchema.Name,
                        strict = true,
                        schema = request.JsonSchema.Schema
                    }
                }
            })
        };

        BuildHeaders(httpRequest);

        var response = await _httpClient.SendAsync(httpRequest);

        if (!response.IsSuccessStatusCode)
        {
            await HandleHttpError(response);
        }

        var stringContent = await response.Content.ReadAsStringAsync();

        var openRouterResponse = await response.Content.ReadFromJsonAsync<OpenRouterResponse>();

        return ParseApiResponse<T>(openRouterResponse);
    }

    private void BuildHeaders(HttpRequestMessage request)
    {
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
    }

    private static async Task HandleHttpError(HttpResponseMessage response)
    {
        var errorContent = await response.Content.ReadAsStringAsync();

        throw new HttpRequestException($"OpenRouter API request failed with status code {response.StatusCode}: {errorContent}");
    }

    private static T ParseApiResponse<T>(OpenRouterResponse? openRouterResponse)
    {
        if (openRouterResponse == null || openRouterResponse.Choices == null || openRouterResponse.Choices.Count == 0)
        {
            throw new InvalidOperationException("OpenRouter API returned an empty response");
        }

        var content = openRouterResponse.Choices[0].Message?.Content;

        if (string.IsNullOrEmpty(content))
        {
            throw new InvalidOperationException("OpenRouter API response does not contain valid content");
        }

        try
        {
            return JsonSerializer.Deserialize<T>(content, options) ??
                throw new InvalidOperationException($"Failed to parse API response as {typeof(T).Name}");
        }
        catch (Exception)
        {
            throw new InvalidOperationException($"Failed to parse API response as {typeof(T).Name}");
        }
    }

    private class OpenRouterResponse
    {
        public List<Choice>? Choices { get; set; }

        public class Choice
        {
            public ChatRole? Role { get; set; }
            public Message? Message { get; set; }
        }

        public class Message
        {
            public string? Content { get; set; }
        }
    }
}