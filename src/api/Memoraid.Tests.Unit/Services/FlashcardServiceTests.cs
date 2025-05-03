using FluentValidation;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
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
    private IValidator<CreateFlashcardsRequest> _createFlashcardsValidator;
    private IValidator<GetFlashcardsRequest> _getFlashcardsValidator;
    private IValidator<long> _deleteFlashcardValidator;
    private Mock<IFlashcardGenerationService> _mockFlashcardGenerationService;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
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

        _createFlashcardsValidator = new CreateFlashcardsRequestValidator(_dbContext);
        _getFlashcardsValidator = new GetFlashcardsRequestValidator();
        _deleteFlashcardValidator = new DeleteFlashcardRequestValidator();

        _mockFlashcardGenerationService = new Mock<IFlashcardGenerationService>();

        _flashcardService = new FlashcardService(
            _dbContext,
            _createFlashcardsValidator,
            _getFlashcardsValidator,
            _deleteFlashcardValidator,
            _mockFlashcardGenerationService.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task CreateFlashcardsAsync_Should_SaveFlashcardsOfManualAndAISourcesAndUpdateGenerationMetrics_When_ValidData()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.Manual },
                new() { Front = "Front2", Back = "Back2", Source = FlashcardSource.AIFull, GenerationId = 123 },
                new() { Front = "Front3", Back = "Back3", Source = FlashcardSource.AIEdited, GenerationId = 123 }
            ]
        };

        _mockFlashcardGenerationService
            .Setup(s => s.UpdateGenerationMetricsAsync(new List<long> { 123 }))
            .Returns(Task.CompletedTask);

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

        _mockFlashcardGenerationService.Verify(
            s => s.UpdateGenerationMetricsAsync(It.Is<IEnumerable<long>>(ids => ids.SequenceEqual(new List<long> { 123 }))),
            Times.Once);
    }

    [Test]
    public async Task CreateFlashcardsAsync_Should_Throw_When_ValidationFails()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front", Back = null, Source = FlashcardSource.Manual }
            ]
        };

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(() => _flashcardService.CreateFlashcardsAsync(request));
    }

    [Test]
    public async Task CreateFlashcardsAsync_Should_NotCallUpdateGenerationMetrics_When_NoGenerationIdsExist()
    {
        // Arrange
        var request = new CreateFlashcardsRequest
        {
            Flashcards =
            [
                new() { Front = "Front1", Back = "Back1", Source = FlashcardSource.Manual },
                new() { Front = "Front2", Back = "Back2", Source = FlashcardSource.Manual }
            ]
        };

        // Act
        await _flashcardService.CreateFlashcardsAsync(request);

        // Assert
        _mockFlashcardGenerationService.Verify(
            s => s.UpdateGenerationMetricsAsync(It.IsAny<IEnumerable<long>>()),
            Times.Never);
    }

    [Test]
    public async Task GetFlashcardsAsync_Should_ReturnFlashcardsOfCurrentUserAndResultShoulBePaginated_When_PaginationParametersAreSpecified()
    {
        // Arrange
        var userId = 1;
        var flashcards = new List<Flashcard>();
        for (int i = 0; i < 20; i++)
        {
            flashcards.Add(new Flashcard
            {
                UserId = userId,
                Front = $"Front {i + 1}",
                Back = $"Back {i + 1}",
                Source = FlashcardSource.Manual,
                CreatedOn = DateTime.UtcNow.AddMinutes(i - 19)
            });
        }

        for (int i = 0; i < 5; i++)
        {
            flashcards.Add(new Flashcard
            {
                UserId = userId + 1,
                Front = $"Other user front {i + 1}",
                Back = $"Other user back {i + 1}",
                Source = FlashcardSource.Manual
            });
        }

        await _dbContext.Flashcards.AddRangeAsync(flashcards);
        await _dbContext.SaveChangesAsync();

        var request = new GetFlashcardsRequest
        {
            PageNumber = 2,
            PageSize = 5
        };

        var expectedFlashcards = new List<GetFlashcardsResponse.FlashcardsListItem>();
        for (int i = 14; i >= 10; i--)
        {
            expectedFlashcards.Add(new GetFlashcardsResponse.FlashcardsListItem(
                flashcards[i].Id,
                flashcards[i].Front,
                flashcards[i].Back
            ));
        }

        // Act
        var result = await _flashcardService.GetFlashcardsAsync(request);

        // Assert
        ShouldHaveReturnedExpectedFlashcards(result, expectedTotalCount: 20, expectedFlashcards);
    }

    [Test]
    public async Task GetFlashcardsAsync_Should_UseDefaultPagination_When_NoPaginationParametersSpecified()
    {
        // Arrange
        var userId = 1;
        var flashcards = new List<Flashcard>();
        for (int i = 0; i < 20; i++)
        {
            flashcards.Add(new Flashcard
            {
                UserId = userId,
                Front = $"Front {i + 1}",
                Back = $"Back {i + 1}",
                Source = FlashcardSource.Manual,
                CreatedOn = DateTime.UtcNow.AddMinutes(i - 19)
            });
        }

        await _dbContext.Flashcards.AddRangeAsync(flashcards);
        await _dbContext.SaveChangesAsync();

        var request = new GetFlashcardsRequest();

        var expectedFlashcards = new List<GetFlashcardsResponse.FlashcardsListItem>();
        for (int i = 19; i >= 10; i--)
        {
            expectedFlashcards.Add(new GetFlashcardsResponse.FlashcardsListItem(
                flashcards[i].Id,
                flashcards[i].Front,
                flashcards[i].Back
            ));
        }

        // Act
        var result = await _flashcardService.GetFlashcardsAsync(request);

        // Assert
        ShouldHaveReturnedExpectedFlashcards(result, expectedTotalCount: 20, expectedFlashcards);
    }

    [Test]
    public async Task GetFlashcardsAsync_Should_ThrowValidationError_When_PageNumberIsLessThanOne()
    {
        // Arrange
        var request = new GetFlashcardsRequest
        {
            PageNumber = 0,
            PageSize = 5
        };

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => _flashcardService.GetFlashcardsAsync(request));
        var errors = exception.Errors.ToList();
        errors.Count.ShouldBe(1);
        errors[0].ErrorMessage.ShouldBe(string.Format(ErrorMessages.GREATER_THAN, nameof(GetFlashcardsRequest.PageNumber), 0));
        errors[0].PropertyName.ShouldBe(nameof(GetFlashcardsRequest.PageNumber));
    }

    [Test]
    public async Task DeleteFlashcardAsync_Should_DeleteFlashcard_When_FlashcardExists()
    {
        // Arrange
        var flashcardId = 1;

        await _dbContext.Flashcards.AddAsync(new Flashcard
        {
            Id = flashcardId,
            UserId = 1,
            Front = "Test Front",
            Back = "Test Back",
            Source = FlashcardSource.Manual
        });

        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _flashcardService.DeleteFlashcardAsync(flashcardId);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var flashcardExists = await _dbContext.Flashcards.AnyAsync(f => f.Id == flashcardId);
        flashcardExists.ShouldBeFalse();
    }

    [Test]
    public async Task DeleteFlashcardAsync_Should_ReturnNotFoundError_When_FlashcardDoesNotExist()
    {
        // Arrange
        var id = 999;

        // Act
        var result = await _flashcardService.DeleteFlashcardAsync(id);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.Length.ShouldBe(1);
        var error = result.Errors.Single();
        error.Code.ShouldBe(IFlashcardService.ErrorCodes.FlashcardNotFound);
        error.Message.ShouldBe(FlashcardService.FlashcardNotFoundMessage);
        error.PropertyName.ShouldBe(nameof(id));
    }

    [Test]
    public async Task DeleteFlashcardAsync_Should_ReturnNotFoundError_When_FlashcardBelongsToAnotherUser()
    {
        // Arrange
        var id = 2;

        await _dbContext.Flashcards.AddAsync(new Flashcard
        {
            Id = id,
            UserId = 2,
            Front = "Other User Front",
            Back = "Other User Back",
            Source = FlashcardSource.Manual
        });

        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _flashcardService.DeleteFlashcardAsync(id);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.Length.ShouldBe(1);
        var error = result.Errors.Single();
        error.Code.ShouldBe(IFlashcardService.ErrorCodes.FlashcardNotFound);
        error.Message.ShouldBe(FlashcardService.FlashcardNotFoundMessage);
        error.PropertyName.ShouldBe(nameof(id));
    }

    [Test]
    public async Task DeleteFlashcardAsync_Should_ReturnValidationError_When_IdIsInvalid()
    {
        // Arrange & Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(
            () => _flashcardService.DeleteFlashcardAsync(0));
        var errors = exception.Errors.ToList();
        errors.Count.ShouldBe(1);
        errors[0].ErrorMessage.ShouldBe(DeleteFlashcardRequestValidator.InvalidFlashcardIdMessage);
    }

    private void ShouldHaveReturnedExpectedFlashcards(Response<GetFlashcardsResponse> response, int expectedTotalCount, List<GetFlashcardsResponse.FlashcardsListItem> expectedFlashcards)
    {
        response.ShouldNotBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Total.ShouldBe(expectedTotalCount);
        response.Data.Items.ShouldNotBeNull();
        response.Data.Items.Count.ShouldBe(expectedFlashcards.Count);

        for (int i = 0; i < response.Data.Items.Count; i++)
        {
            var actualFlashcard = response.Data.Items[i];
            var expectedFlashcard = expectedFlashcards[i];

            actualFlashcard.Id.ShouldBe(expectedFlashcard.Id);
            actualFlashcard.Front.ShouldBe(expectedFlashcard.Front);
            actualFlashcard.Back.ShouldBe(expectedFlashcard.Back);
        }
    }
}
