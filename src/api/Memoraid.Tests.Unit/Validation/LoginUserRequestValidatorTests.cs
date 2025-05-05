using FluentValidation.TestHelper;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Validation;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class LoginUserRequestValidatorTests
{
    private LoginUserRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new LoginUserRequestValidator();
    }

    [Test]
    public async Task ValidateRequest_Should_NotHaveError_When_RequestIsValid()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Email = "valid@example.com",
            Password = "validpassword"
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task ValidateEmail_Should_HaveError_When_EmailIsNullOrEmpty(string? email)
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Email = email,
            Password = "password123"
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(string.Format(ErrorMessages.REQUIRED, nameof(LoginUserRequest.Email)));
    }

    [Test]
    public async Task ValidateEmail_Should_HaveError_When_EmailFormatIsInvalid()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Email = "invalid-email",
            Password = "password123"
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(LoginUserRequestValidator.InvalidEmail);
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task ValidatePassword_Should_HaveError_When_PasswordIsNullOrEmpty(string? password)
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Email = "test@example.com",
            Password = password
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(string.Format(ErrorMessages.REQUIRED, nameof(LoginUserRequest.Password)));
    }
}