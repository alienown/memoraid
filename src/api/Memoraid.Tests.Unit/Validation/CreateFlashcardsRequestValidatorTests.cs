using FluentValidation.TestHelper;
using Memoraid.Tests.Unit.Common;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Memoraid.WebApi.Constants.ErrorMessages;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class CreateFlashcardsRequestValidatorTests
{
    private CreateFlashcardsRequestValidator _validator;
    private MemoraidDbContext _dbContext;

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
            Id = 1,
            UserId = 1,
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
            UserId = 1,
            AIModel = "TestModel3",
            SourceText = "Test text 3"
        });

        await _dbContext.SaveChangesAsync();

        _validator = new CreateFlashcardsRequestValidator(_dbContext);
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
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.Manual },
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.AIEdited, GenerationId = 1 },
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.AIFull, GenerationId = 1 }
            }
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
            .WithErrorMessage(CreateFlashcardsRequestValidator.FlashcardsAreRequired)
            .WithPropertyName(nameof(CreateFlashcardsRequest.Flashcards));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_FlashcardsIsEmpty()
    {
        // Arrange
        var request = new CreateFlashcardsRequest { Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>() };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Flashcards)
            .WithErrorMessage(CreateFlashcardsRequestValidator.FlashcardsAreRequired)
            .WithPropertyName(nameof(CreateFlashcardsRequest.Flashcards));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_FrontIsNull()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = null, Back = "Back", Source = FlashcardSource.Manual }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Front")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Front)))
            .WithPropertyName("Flashcards[0].Front");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_FrontExceedsMaxLength()
    {
        // Arrange
        var longFront = new string('a', 501);
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = longFront, Back = "Back", Source = FlashcardSource.Manual }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Front")
            .WithErrorMessage(string.Format(MAX_LENGTH, nameof(CreateFlashcardsRequest.CreateFlashcardData.Front), 500))
            .WithPropertyName("Flashcards[0].Front");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_BackIsNull()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = null, Source = FlashcardSource.Manual }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Back")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Back)))
            .WithPropertyName("Flashcards[0].Back");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_BackExceedsMaxLength()
    {
        // Arrange
        var longBack = new string('a', 201);
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = longBack, Source = FlashcardSource.Manual }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Back")
            .WithErrorMessage(string.Format(MAX_LENGTH, nameof(CreateFlashcardsRequest.CreateFlashcardData.Back), 200))
            .WithPropertyName("Flashcards[0].Back");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_SourceIsNull()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = "Back", Source = null }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Source")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Source)))
            .WithPropertyName("Flashcards[0].Source");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_SourceIsNotDefined()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = "Back", Source = (FlashcardSource)2137 }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].Source")
            .WithErrorMessage(CreateFlashcardsRequestValidator.InvalidSourceError)
            .WithPropertyName("Flashcards[0].Source");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_ManualFlashcardHasGenerationId()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.Manual, GenerationId = 1 }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].GenerationId")
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationIdMustBeNullForFlashcardsWithManualSource)
            .WithPropertyName("Flashcards[0].GenerationId");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIFlashcardHasNoGenerationId()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = null }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].GenerationId")
            .WithErrorMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId)))
            .WithPropertyName("Flashcards[0].GenerationId");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIFlashcardGenerationIdIsLessThanOrEqualToZero()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = 0 }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor("Flashcards[0].GenerationId")
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationIdMustBeGreaterThanZeroError)
            .WithPropertyName("Flashcards[0].GenerationId");
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIGenerationDoesNotExist()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = 10 }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId))
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationNotExistsError)
            .WithPropertyName(nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_AIGenerationBelongsToAnotherUser()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards = new List<CreateFlashcardsRequest.CreateFlashcardData>
            {
                new() { Front = "Front", Back = "Back", Source = FlashcardSource.AIFull, GenerationId = 2 }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId))
            .WithErrorMessage(CreateFlashcardsRequestValidator.GenerationNotExistsError)
            .WithPropertyName(nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId));
    }
}
