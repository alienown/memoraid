using FluentValidation.TestHelper;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Validation;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class GetFlashcardsRequestValidatorTests
{
    private GetFlashcardsRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetFlashcardsRequestValidator();
    }

    [Test]
    public async Task Validate_Should_NotHaveError_When_NoParametersProvided()
    {
        // Arrange
        var request = new GetFlashcardsRequest();

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Validate_Should_NotHaveError_When_ValidParametersProvided()
    {
        // Arrange
        var request = new GetFlashcardsRequest
        {
            PageNumber = 1,
            PageSize = 20
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Validate_Should_HaveError_When_PageNumberIsLessThanOne()
    {
        // Arrange
        var request = new GetFlashcardsRequest
        {
            PageNumber = 0,
            PageSize = 10
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorMessage(string.Format(ErrorMessages.GREATER_THAN, nameof(GetFlashcardsRequest.PageNumber), 0));
    }

    [Test]
    public async Task Validate_Should_HaveError_When_PageSizeIsLessThanOne()
    {
        // Arrange
        var request = new GetFlashcardsRequest
        {
            PageNumber = 1,
            PageSize = 0
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage(string.Format(ErrorMessages.GREATER_THAN, nameof(GetFlashcardsRequest.PageSize), 0));
    }
}