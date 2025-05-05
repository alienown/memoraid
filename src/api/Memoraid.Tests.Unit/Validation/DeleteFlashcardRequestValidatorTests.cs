using FluentValidation.TestHelper;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Validation;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class DeleteFlashcardRequestValidatorTests
{
    private DeleteFlashcardRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new DeleteFlashcardRequestValidator();
    }

    [Test]
    public void Validate_Should_NotHaveError_When_IdIsValid()
    {
        // Arrange
        long id = 1;

        // Act
        var result = _validator.TestValidate(id);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Validate_Should_HaveError_When_IdIsZero(long id)
    {
        // Arrange & Act
        var result = _validator.TestValidate(id);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(string.Format(ErrorMessages.GREATER_THAN, nameof(id), 0));
    }
}