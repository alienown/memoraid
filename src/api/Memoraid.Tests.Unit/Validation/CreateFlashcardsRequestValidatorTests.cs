using FluentValidation.TestHelper;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using static Memoraid.WebApi.Constants.ErrorMessages;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class CreateFlashcardsRequestValidatorTests
{
    private const long TEST_USER_ID = 1;

    private CreateFlashcardsRequestValidator _validator;
    private MemoraidDbContext _dbContext;
    private Mock<IUserContext> _mockUserContext;

    [SetUp]
    public async Task Setup()
    {
        _mockUserContext = new Mock<IUserContext>();
        _mockUserContext.Setup(x => x.UserId).Returns(TEST_USER_ID);
        _mockUserContext.Setup(x => x.GetUserIdOrThrow()).Returns(TEST_USER_ID);

        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor(_mockUserContext.Object))
            .Options;

        _dbContext = new MemoraidDbContext(options);

        _dbContext.FlashcardAIGenerations.Add(new FlashcardAIGeneration
        {
            Id = 1,
            UserId = TEST_USER_ID,
            AIModel = "TestModel1",
            SourceText = "Test text 1"
        });

        _dbContext.FlashcardAIGenerations.Add(new FlashcardAIGeneration
        {
            Id = 2,
            UserId = 2,
            AIModel = "TestModel2",
            SourceText = "Test text 2"
        });

        _dbContext.FlashcardAIGenerations.Add(new FlashcardAIGeneration
        {
            Id = 3,
            UserId = TEST_USER_ID,
            AIModel = "TestModel3",
            SourceText = "Test text 3"
        });

        await _dbContext.SaveChangesAsync();

        _validator = new CreateFlashcardsRequestValidator(_mockUserContext.Object, _dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task Validate_Should_NotHaveError_When_RequestIsValid()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.Manual },
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.AIEdited, GenerationId = 1 },
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.AIFull, GenerationId = 1 }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Validate_Should_HaveError_When_FlashcardsIsNull()
    {
        // Arrange
        var request = new CreateFlashcardsRequest { Flashcards = null };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Flashcards)
            .WithErrorMessage(CreateFlashcardsRequestValidator.FlashcardsAreRequired);
    }

    [Test]
    public async Task Validate_Should_HaveError_When_FlashcardsIsEmpty()
    {
        // Arrange
        var request = new CreateFlashcardsRequest { Flashcards = [] };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Flashcards)
            .WithErrorMessage(CreateFlashcardsRequestValidator.FlashcardsAreRequired);
    }

    [Test]
    public async Task Validate_Should_HaveError_When_FrontIsNull()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = null, Back = "Back", Source = FlashcardSource.Manual }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Front")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Front)));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_FrontExceedsMaxLength()
    {
        // Arrange
        var longFront = new string('a', 501);
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = longFront, Back = "Back", Source = FlashcardSource.Manual }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Front")
            .WithErrorMessage(string.Format(MAX_LENGTH, nameof(CreateFlashcardsRequest.CreateFlashcardData.Front), 500));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_BackIsNull()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = null, Source = FlashcardSource.Manual }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Back")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Back)));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_BackExceedsMaxLength()
    {
        // Arrange
        var longBack = new string('a', 201);
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = longBack, Source = FlashcardSource.Manual }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Back")
            .WithErrorMessage(string.Format(MAX_LENGTH, nameof(CreateFlashcardsRequest.CreateFlashcardData.Back), 200));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_SourceIsNull()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = "Back", Source = null }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Source")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Source)));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_SourceIsNotDefined()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = "Back", Source = (FlashcardSource)2137 }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Source")
            .WithErrorMessage(CreateFlashcardsRequestValidator.InvalidSourceError);
    }

    [Test]
    public async Task Validate_Should_HaveError_When_ManualFlashcardHasGenerationId()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.Manual, GenerationId = 1 }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].GenerationId")
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationIdMustBeNullForFlashcardsWithManualSource);
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIFlashcardHasNoGenerationId()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = null }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].GenerationId")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId)));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIFlashcardGenerationIdIsLessThanOrEqualToZero()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = 0 }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].GenerationId")
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationIdMustBeGreaterThanZeroError);
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIGenerationDoesNotExist()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = 10 }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId))
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationNotExistsError);
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIGenerationBelongsToAnotherUser()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = 2 }
            ]
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId))
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationNotExistsError);
    }
}
