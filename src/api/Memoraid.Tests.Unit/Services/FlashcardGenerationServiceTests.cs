using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
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
}
