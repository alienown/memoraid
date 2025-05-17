using Memoraid.WebApi.Services;
using Memoraid.WebApi.Services.OpenRouter;
using Shouldly;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Services;

[TestFixture]
public class MockOpenRouterServiceTests
{
    private MockOpenRouterService _service;

    [SetUp]
    public void Setup()
    {
        _service = new MockOpenRouterService();
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_GenerateRandomFlashcards_When_RequestIsValid()
    {
        // Arrange
        var request = new CompleteWithStructuredOutputRequest
        {
            Model = "any-model",
            Messages =
            [
                new() { Role = ChatRole.System, Content = "System message" },
                new() { Role = ChatRole.User, Content = "User message" }
            ],
            JsonSchema = new()
            {
                Name = "flashcardGeneration",
                Schema = System.Text.Json.JsonDocument.Parse("{}").RootElement
            }
        };

        // Act
        var result = await _service.CompleteWithStructuredOutputAsync<FlashcardGenerationService.FlashcardGenerationResult>(request);

        // Assert
        result.ShouldNotBeNull();
        result.Flashcards.ShouldNotBeNull();
        result.Flashcards.Count.ShouldBeGreaterThanOrEqualTo(3);
        result.Flashcards.Count.ShouldBeLessThanOrEqualTo(8);

        foreach (var flashcard in result.Flashcards)
        {
            flashcard.Front.ShouldNotBeNullOrEmpty();
            flashcard.Back.ShouldNotBeNullOrEmpty();
        }
    }

    [Test]
    public async Task CompleteWithStructuredOutputAsync_Should_ThrowNotImplementedException_When_UnsupportedTypeRequested()
    {
        // Arrange
        var request = new CompleteWithStructuredOutputRequest
        {
            Model = "any-model",
            Messages =
            [
                new() { Role = ChatRole.System, Content = "System message" },
                new() { Role = ChatRole.User, Content = "User message" }
            ],
            JsonSchema = new()
            {
                Name = "unknownSchema",
                Schema = System.Text.Json.JsonDocument.Parse("{}").RootElement
            }
        };

        // Arrange Act & Assert
        await Should.ThrowAsync<System.NotImplementedException>(async () =>
        {
            await _service.CompleteWithStructuredOutputAsync<object>(request);
        });
    }
}
