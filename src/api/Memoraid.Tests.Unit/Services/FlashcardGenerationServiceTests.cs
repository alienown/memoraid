using FluentValidation;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Services;

[TestFixture]
public class FlashcardGenerationServiceTests
{
    private FlashcardGenerationService _service;
    private MemoraidDbContext _dbContext;
    private Mock<IOpenRouterService> _mockOpenRouterService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor())
            .Options;

        _dbContext = new MemoraidDbContext(options);
        _mockOpenRouterService = new Mock<IOpenRouterService>();

        _service = new FlashcardGenerationService(
            _dbContext,
            _mockOpenRouterService.Object,
            new GenerateFlashcardsRequestValidator());
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    [Test]
    public async Task GenerateFlashcardsAsync_Should_PersistGenerationLogAndReturnFlashcards_When_RequestIsValid()
    {
        // Arrange
        var request = new GenerateFlashcardsRequest { SourceText = "Valid text" };
        var flashcards = new List<FlashcardGenerationService.FlashcardGenerationResult.Flashcard>
        {
            new() { Front = "Question 1", Back = "Answer 1" },
            new() { Front = "Question 2", Back = "Answer 2" }
        };
        var result = new FlashcardGenerationService.FlashcardGenerationResult { Flashcards = flashcards };

        SetupCompleteWithStructuredOutputAsync(request).ReturnsAsync(result);

        // Act
        var response = await _service.GenerateFlashcardsAsync(request);

        // Assert
        response.GenerationId.ShouldBeGreaterThan(0);
        response.Flashcards.ShouldNotBeNull();
        response.Flashcards.Count.ShouldBe(flashcards.Count);
        response.Flashcards[0].Front.ShouldBe(flashcards[0].Front);
        response.Flashcards[0].Back.ShouldBe(flashcards[0].Back);
        response.Flashcards[1].Front.ShouldBe(flashcards[1].Front);
        response.Flashcards[1].Back.ShouldBe(flashcards[1].Back);

        var log = _dbContext.FlashcardAIGenerations.FirstOrDefault(g => g.Id == response.GenerationId);
        log.ShouldNotBeNull();
        log.AIModel.ShouldBe(FlashcardGenerationService.AIModel);
        log.SourceText.ShouldBe(request.SourceText);
        log.AcceptedEditedFlashcardsCount.ShouldBeNull();
        log.AcceptedUneditedFlashcardsCount.ShouldBeNull();
        log.AllFlashcardsCount.ShouldBe(response.Flashcards.Count);
    }

    [Test]
    public async Task GenerateFlashcardsAsync_Should_ThrowValidationException_When_RequestIsInvalid()
    {
        // Arrange
        var request = new GenerateFlashcardsRequest { SourceText = "" };

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(async () =>
        {
            await _service.GenerateFlashcardsAsync(request);
        });
    }

    [Test]
    public async Task GenerateFlashcardsAsync_Should_ThrowValidationException_When_OpenRouterServiceFails()
    {
        // Arrange
        var request = new GenerateFlashcardsRequest { SourceText = "Valid text" };

        SetupCompleteWithStructuredOutputAsync(request).ThrowsAsync(new Exception("API error"));

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(async () =>
        {
            await _service.GenerateFlashcardsAsync(request);
        });

        exception.Errors.Count().ShouldBe(1);
        exception.Errors.ShouldContain(e => e.ErrorMessage == "Failed to generate flashcards");
    }

    [Test]
    public async Task UpdateGenerationMetricsAsync_Should_HandleMultipleGenerations_When_MultipleGenerationIdsProvided()
    {
        // Arrange
        var generationId1 = 1L;
        var generationId2 = 2L;

        var generations = new List<FlashcardAIGeneration>
        {
            new() {
                Id = generationId1,
                UserId = 1,
                AIModel = "TestModel1",
                SourceText = "Test text 1"
            },
            new() {
                Id = generationId2,
                UserId = 1,
                AIModel = "TestModel2",
                SourceText = "Test text 2"
            }
        };

        await _dbContext.FlashcardAIGenerations.AddRangeAsync(generations);

        var flashcards = new List<Flashcard>
        {
            new() { UserId = 1, Front = "Gen1-1", Back = "Back1", Source = FlashcardSource.AIFull, FlashcardAIGenerationId = generationId1 },
            new() { UserId = 1, Front = "Gen1-2", Back = "Back2", Source = FlashcardSource.AIEdited, FlashcardAIGenerationId = generationId1 },
            new() { UserId = 1, Front = "Gen2-1", Back = "Back3", Source = FlashcardSource.AIFull, FlashcardAIGenerationId = generationId2 },
            new() { UserId = 1, Front = "Gen2-2", Back = "Back4", Source = FlashcardSource.AIFull, FlashcardAIGenerationId = generationId2 },
            new() { UserId = 1, Front = "Gen2-3", Back = "Back5", Source = FlashcardSource.AIEdited, FlashcardAIGenerationId = generationId2 }
        };

        await _dbContext.Flashcards.AddRangeAsync(flashcards);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.UpdateGenerationMetricsAsync(new List<long> { generationId1, generationId2 });

        // Assert
        var updatedGeneration1 = generations[0];
        updatedGeneration1.AllFlashcardsCount.ShouldBe(2);
        updatedGeneration1.AcceptedUneditedFlashcardsCount.ShouldBe(1);
        updatedGeneration1.AcceptedEditedFlashcardsCount.ShouldBe(1);

        var updatedGeneration2 = generations[1];
        updatedGeneration2.AllFlashcardsCount.ShouldBe(3);
        updatedGeneration2.AcceptedUneditedFlashcardsCount.ShouldBe(2);
        updatedGeneration2.AcceptedEditedFlashcardsCount.ShouldBe(1);
    }

    [Test]
    public async Task UpdateGenerationMetricsAsync_Should_DoNothing_When_EmptyList()
    {
        // Arrange & Act & Assert
        await _service.UpdateGenerationMetricsAsync(new List<long>());
    }

    [Test]
    public async Task UpdateGenerationMetricsAsync_Should_HandleGracefully_When_GenerationDoesNotExist()
    {
        // Arrange & Act & Assert
        await _service.UpdateGenerationMetricsAsync(new List<long> { 999 });
    }

    [Test]
    public async Task GenerateFlashcardsAsync_Should_TruncateLongContent_When_ContentExceedsLimits()
    {
        // Arrange
        var request = new GenerateFlashcardsRequest { SourceText = "Valid text" };

        var longFrontText = new string('A', 600);
        var longBackText = new string('B', 250);

        var flashcards = new List<FlashcardGenerationService.FlashcardGenerationResult.Flashcard>
        {
            new() { Front = new string('A', 600), Back = new string('B', 250) },
            new() { Front = "Short question", Back = "Short answer" }
        };
        var result = new FlashcardGenerationService.FlashcardGenerationResult { Flashcards = flashcards };

        SetupCompleteWithStructuredOutputAsync(request).ReturnsAsync(result);

        // Act
        var response = await _service.GenerateFlashcardsAsync(request);

        // Assert
        response.Flashcards[0].Front.ShouldBe(longFrontText[..500]);
        response.Flashcards[0].Back.ShouldBe(longBackText[..200]);
        response.Flashcards[1].Front.ShouldBe("Short question");
        response.Flashcards[1].Back.ShouldBe("Short answer");
    }

    private Moq.Language.Flow.ISetup<IOpenRouterService, Task<FlashcardGenerationService.FlashcardGenerationResult>> SetupCompleteWithStructuredOutputAsync(GenerateFlashcardsRequest request)
    {
        var expectedJsonSchema = @"{
            ""type"": ""object"",
            ""properties"": {
                ""flashcards"": {
                    ""type"": ""array"",
                    ""items"": {
                        ""type"": ""object"",
                        ""properties"": {
                            ""front"": { ""type"": ""string"" },
                            ""back"": { ""type"": ""string"" }
                        },
                        ""required"": [""front"", ""back""],
                        ""additionalProperties"": false
                    }
                }
            },
            ""required"": [""flashcards""],
            ""additionalProperties"": false
        }";

        // Normalize the expected schema for comparison
        var expectedSchema = JsonSerializer.Serialize(
            JsonSerializer.Deserialize<JsonElement>(expectedJsonSchema),
            new JsonSerializerOptions { WriteIndented = false });

        return _mockOpenRouterService
            .Setup(s => s.CompleteWithStructuredOutputAsync<FlashcardGenerationService.FlashcardGenerationResult>(
                It.Is<CompleteWithStructuredOutputRequest>(req =>
                    req.Model == FlashcardGenerationService.AIModel &&
                    req.Messages.Count == 2 &&
                    req.Messages[0].Role == ChatRole.System &&
                    req.Messages[0].Content == FlashcardGenerationService.GenerationPrompt &&
                    req.Messages[1].Role == ChatRole.User &&
                    req.Messages[1].Content == $"<sourceText>{request.SourceText}</sourceText>" &&
                    req.JsonSchema != null &&
                    req.JsonSchema.Name == "flashcardGeneration" &&
                    JsonSerializer.Serialize(req.JsonSchema.Schema, new JsonSerializerOptions { WriteIndented = false }) == expectedSchema
                )
            ));
    }
}
