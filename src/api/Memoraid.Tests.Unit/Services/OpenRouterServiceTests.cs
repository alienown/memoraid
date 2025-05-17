using Memoraid.WebApi.Configuration;
using Memoraid.WebApi.Services.OpenRouter;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Services;

[TestFixture]
public class OpenRouterServiceTests
{
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    private OpenRouterService _openRouterService;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://openrouter.ai")
        };

        var optionsMock = new Mock<IOptions<ApplicationOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new ApplicationOptions
        {
            OpenRouter = new OpenRouter
            {
                UseMock = false,
                ApiKey = "test_api_key",
                ApiBaseUrl = "https://openrouter.ai/api/v1"
            }
        });

        _openRouterService = new OpenRouterService(_httpClient, optionsMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_ReturnDeserializedResponse_When_ApiReturnsSuccessfulResponse()
    {
        // Arrange
        var weatherInfo = GetWeatherInfo();

        var apiResponse = GetApiResponse(weatherInfo);

        SetupMockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(apiResponse));

        var request = GetCompleteWithStructuredOutputRequest();

        // Act
        var result = await _openRouterService.CompleteWithStructuredOutputAsync<WeatherInfo>(request);

        // Assert
        result.ShouldBeEquivalentTo(weatherInfo);
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_ThrowHttpRequestException_When_ApiReturnsErrorResponse()
    {
        // Arrange
        SetupMockHttpMessageHandler(HttpStatusCode.BadRequest, "Invalid request");

        var request = GetCompleteWithStructuredOutputRequest();

        // Act & Assert
        var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
            await _openRouterService.CompleteWithStructuredOutputAsync<WeatherInfo>(request));

        exception.Message.ShouldContain("OpenRouter API request failed with status code BadRequest");
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_ThrowInvalidOperationException_When_ApiReturnsEmptyResponse()
    {
        // Arrange
        var emptyResponse = new { choices = Array.Empty<object>() };
        SetupMockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(emptyResponse));

        var request = GetCompleteWithStructuredOutputRequest();

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidOperationException>(async () =>
            await _openRouterService.CompleteWithStructuredOutputAsync<WeatherInfo>(request));

        exception.Message.ShouldBe("OpenRouter API returned an empty response");
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_ThrowInvalidOperationException_When_ResponseContentIsNull()
    {
        // Arrange
        var nullContentResponse = new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = (string?)null
                    }
                }
            }
        };

        SetupMockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(nullContentResponse));

        var request = GetCompleteWithStructuredOutputRequest();

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidOperationException>(async () =>
            await _openRouterService.CompleteWithStructuredOutputAsync<WeatherInfo>(request));

        exception.Message.ShouldBe("OpenRouter API response does not contain valid content");
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_ThrowInvalidOperationException_When_ResponseContainsMalformedJson()
    {
        // Arrange
        var malformedJsonResponse = new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = "{\"Location\":\"London\", \"Temperature\":\"invalid_number\", \"Conditions\":\"Cloudy\"}"
                    }
                }
            }
        };

        SetupMockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(malformedJsonResponse));

        var request = GetCompleteWithStructuredOutputRequest();

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidOperationException>(async () =>
            await _openRouterService.CompleteWithStructuredOutputAsync<WeatherInfo>(request));

        exception.Message.ShouldBe($"Failed to parse API response as {typeof(WeatherInfo).Name}");
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_SetAuthorizationHeader_When_MakingRequest()
    {
        // Arrange
        var weatherInfo = GetWeatherInfo();

        var apiResponse = GetApiResponse(weatherInfo);

        SetupMockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(apiResponse),
            request =>
            {
                return "Bearer test_api_key" == request.Headers.Authorization?.ToString();
            });

        var request = GetCompleteWithStructuredOutputRequest();

        // Act & Assert
        await Should.NotThrowAsync(async () => await _openRouterService.CompleteWithStructuredOutputAsync<WeatherInfo>(request));
    }

    private void SetupMockHttpMessageHandler(HttpStatusCode statusCode, string content, Func<HttpRequestMessage, bool>? requestExpectation = null)
    {
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => requestExpectation != null ? requestExpectation(req) : true),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            });
    }

    private static CompleteWithStructuredOutputRequest GetCompleteWithStructuredOutputRequest()
    {
        return new CompleteWithStructuredOutputRequest
        {
            Model = "openai/gpt-4o-mini",
            Messages =
            [
                new() { Role = ChatRole.User, Content = "What's the weather in London?" }
            ],
            JsonSchema = new JsonSchemaFormat
            {
                Name = "weatherInfo",
                Schema = JsonDocument.Parse(@"{
                    ""type"": ""object"",
                    ""properties"": {
                        ""location"": { ""type"": ""string"" },
                        ""temperature"": { ""type"": ""number"" },
                        ""conditions"": { ""type"": ""string"" }
                    },
                    ""required"": [""location"", ""temperature"", ""conditions""]
                }").RootElement
            }
        };
    }

    private static WeatherInfo GetWeatherInfo()
    {
        return new WeatherInfo
        {
            Location = "London",
            Temperature = 20.5,
            Conditions = "Cloudy"
        };
    }

    private static object GetApiResponse(WeatherInfo weatherInfo)
    {
        return new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = JsonSerializer.Serialize(weatherInfo)
                    }
                }
            }
        };
    }

    private class WeatherInfo
    {
        public required string Location { get; set; }
        public double Temperature { get; set; }
        public required string Conditions { get; set; }
    }
}