using FluentValidation;
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
public class FlashcardServiceTests
{
    private FlashcardService _flashcardService;
    private MemoraidDbContext _dbContext;
    private IValidator<CreateFlashcardsRequest> _validator;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor())
            .Options;

        _dbContext = new MemoraidDbContext(options);

        _dbContext.FlashcardAIGenerations.Add(new FlashcardAIGeneration
        {
            Id = 123,
            UserId = 1,
            AIModel = "TestModel1",
            SourceText = "Test text 1"
        });

        await _dbContext.SaveChangesAsync();

        _validator = new CreateFlashcardsRequestValidator(_dbContext);

        _flashcardService = new FlashcardService(_dbContext, _validator);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task CreateFlashcardsAsync_Should_SaveFlashcardsOfManualAndAISources_When_GenerationWithGivenIdExists()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.Manual },
                new() { Front = "Front2", Back = "Back2", Source = FlashcardSource.AIFull, GenerationId = 123 },
                new() { Front = "Front3", Back = "Back3", Source = FlashcardSource.AIEdited, GenerationId = 123 }
            }
        };

        // Act
        await _flashcardService.CreateFlashcardsAsync(request);

        // Assert
        var savedFlashcards = _dbContext.Flashcards.ToList();

        savedFlashcards.Count.ShouldBe(3);

        var manualFlashcard = savedFlashcards.FirstOrDefault(f => f.Front == "Front1");
        manualFlashcard.ShouldNotBeNull();
        manualFlashcard.Back.ShouldBe("Back1");
        manualFlashcard.Source.ShouldBe(FlashcardSource.Manual);
        manualFlashcard.FlashcardAIGenerationId.ShouldBeNull();
        // TODO: assert user ID once we got auth
        manualFlashcard.UserId.ShouldBe(1);

        var aiFullFlashcard = savedFlashcards.FirstOrDefault(f => f.Front == "Front2");
        aiFullFlashcard.ShouldNotBeNull();
        aiFullFlashcard.Back.ShouldBe("Back2");
        aiFullFlashcard.Source.ShouldBe(FlashcardSource.AIFull);
        aiFullFlashcard.FlashcardAIGenerationId.ShouldBe(123);
        // TODO: assert user ID once we got auth
        aiFullFlashcard.UserId.ShouldBe(1);

        var aiEditedFlashcard = savedFlashcards.FirstOrDefault(f => f.Front == "Front3");
        aiEditedFlashcard.ShouldNotBeNull();
        aiEditedFlashcard.Back.ShouldBe("Back3");
        aiEditedFlashcard.Source.ShouldBe(FlashcardSource.AIEdited);
        aiEditedFlashcard.FlashcardAIGenerationId.ShouldBe(123);
        // TODO: assert user ID once we got auth
        aiEditedFlashcard.UserId.ShouldBe(1);
    }

    [Test]
    public void CreateFlashcardsAsync_Should_Throw_When_ValidationFails()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = null, Source = FlashcardSource.Manual }
            }
        };

        // Act & Assert
        Should.ThrowAsync<ValidationException>(() =>
            _flashcardService.CreateFlashcardsAsync(request));
    }
}
