using FluentValidation.TestHelper;
using Memoraid.Tests.Unit.Common;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Validation;
using static Memoraid.WebApi.Constants.ErrorMessages;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class GenerateFlashcardsRequestValidatorTests
{
    private GenerateFlashcardsRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GenerateFlashcardsRequestValidator();
    }

    [Test]
    public void Validate_Should_NotHaveError_When_SourceTextIsValid()
    {
        // Arrange
        var request = new GenerateFlashcardsRequest { SourceText = "Some valid text" };

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.SourceText);
    }

    [Test]
    public void Validate_Should_HaveError_When_SourceTextIsEmpty()
    {
        // Arrange
        var request = new GenerateFlashcardsRequest { SourceText = "" };

        // Act & Assert
        var result = _validator.TestValidate(request);

        ShouldHaveRequiredErrorForSourceText(result);
    }

    [Test]
    public void Validate_Should_HaveError_When_SourceTextIsNull()
    {
        // Arrange
        var request = new GenerateFlashcardsRequest { SourceText = null };

        // Act & Assert
        var result = _validator.TestValidate(request);

        ShouldHaveRequiredErrorForSourceText(result);
    }

    [Test]
    public void Validate_Should_HaveError_When_SourceTextExceedsMaxLength()
    {
        // Arrange
        var longText = new string('a', 10001);
        var request = new GenerateFlashcardsRequest { SourceText = longText };

        // Act & Assert
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.SourceText)
            .WithErrorMessage(string.Format(MAX_LENGTH, GenerateFlashcardsRequestValidator.SourceTextFieldName, 10000))
            .WithErrorCode("MaximumLengthValidator")
            .WithPropertyName(nameof(GenerateFlashcardsRequest.SourceText));
    }

    private static void ShouldHaveRequiredErrorForSourceText(TestValidationResult<GenerateFlashcardsRequest> result)
    {
        result.ShouldHaveValidationErrorFor(r => r.SourceText)
            .WithErrorMessage(string.Format(REQUIRED, GenerateFlashcardsRequestValidator.SourceTextFieldName))
            .WithErrorCode("NotEmptyValidator")
            .WithPropertyName(nameof(GenerateFlashcardsRequest.SourceText));
    }
}
