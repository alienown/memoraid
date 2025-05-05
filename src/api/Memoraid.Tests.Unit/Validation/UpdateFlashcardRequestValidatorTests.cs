using FluentValidation.TestHelper;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Validation;
using static Memoraid.WebApi.Constants.ErrorMessages;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class UpdateFlashcardRequestValidatorTests
{
    private UpdateFlashcardRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateFlashcardRequestValidator();
    }

    [Test]
    public void Validate_Should_NotHaveError_When_ValidRequestProvided()
    {
        // Arrange
        var request = new UpdateFlashcardRequest
        {
            Front = "Valid front content",
            Back = "Valid back content"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase(null)]
    [TestCase("")]
    public void Validate_Should_HaveError_When_FrontIsNullOrEmpty(string? front)
    {
        // Arrange
        var request = new UpdateFlashcardRequest
        {
            Front = front,
            Back = "Valid back content"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Front)
            .WithErrorMessage(string.Format(REQUIRED, nameof(UpdateFlashcardRequest.Front)));
    }

    [Test]
    public void Validate_Should_HaveError_When_FrontExceedsMaxLength()
    {
        // Arrange
        var longFront = new string('a', 501);
        var request = new UpdateFlashcardRequest
        {
            Front = longFront,
            Back = "Valid back content"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Front)
            .WithErrorMessage(string.Format(MAX_LENGTH, nameof(UpdateFlashcardRequest.Front), 500));
    }

    [TestCase(null)]
    [TestCase("")]
    public void Validate_Should_HaveError_When_BackIsNullOrEmpty(string? back)
    {
        // Arrange
        var request = new UpdateFlashcardRequest
        {
            Front = "Valid front content",
            Back = back
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Back)
            .WithErrorMessage(string.Format(REQUIRED, nameof(UpdateFlashcardRequest.Back)));
    }

    [Test]
    public void Validate_Should_HaveError_When_BackExceedsMaxLength()
    {
        // Arrange
        var longBack = new string('a', 201);
        var request = new UpdateFlashcardRequest
        {
            Front = "Valid front content",
            Back = longBack
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Back)
            .WithErrorMessage(string.Format(MAX_LENGTH, nameof(UpdateFlashcardRequest.Back), 200));
    }
}