using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Services;

[TestFixture]
public class FlashcardGenerationServiceTests
{
    private FlashcardGenerationService _service;
    private MemoraidDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor())
            .Options;

        _dbContext = new MemoraidDbContext(options);

        _service = new FlashcardGenerationService(_dbContext, new GenerateFlashcardsRequestValidator());
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

        // Act
        var response = await _service.GenerateFlashcardsAsync(request);

        // Assert
        response.GenerationId.ShouldBeGreaterThan(0);
        response.Flashcards.ShouldNotBeNull();
        response.Flashcards.ShouldAllBe(f => !string.IsNullOrWhiteSpace(f.Back) && !string.IsNullOrWhiteSpace(f.Front));

        var log = _dbContext.FlashcardAIGenerations.FirstOrDefault(g => g.Id == response.GenerationId);
        log.ShouldNotBeNull();
        // TODO: assert user ID once we got auth
        log.AIModel.ShouldNotBeNullOrEmpty();
        log.SourceText.ShouldBe("Valid text");
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
        await Should.ThrowAsync<FluentValidation.ValidationException>(async () =>
        {
            await _service.GenerateFlashcardsAsync(request);
        });
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
        await _service.UpdateGenerationMetricsAsync([generationId1, generationId2]);
        
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
        await _service.UpdateGenerationMetricsAsync([]);
    }
    
    [Test]
    public async Task UpdateGenerationMetricsAsync_Should_HandleGracefully_When_GenerationDoesNotExist()
    {
        // Arrange & Act & Assert
        await _service.UpdateGenerationMetricsAsync([999]);
    }
}
