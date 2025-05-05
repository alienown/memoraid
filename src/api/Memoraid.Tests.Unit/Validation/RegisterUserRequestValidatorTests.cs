using FluentValidation.TestHelper;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Validation;

[TestFixture]
public class RegisterUserRequestValidatorTests
{
    private RegisterUserRequestValidator _validator;
    private MemoraidDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor())
            .Options;

        _dbContext = new MemoraidDbContext(options);
        _validator = new RegisterUserRequestValidator(_dbContext);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task ValidateRequest_Should_NotHaveError_When_RequestIsValid()
    {
        // Arrange
        var request = new RegisterUserRequest
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
        var request = new RegisterUserRequest
        {
            Email = email,
            Password = "password123"
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(string.Format(ErrorMessages.REQUIRED, nameof(RegisterUserRequest.Email)));
    }

    [Test]
    public async Task ValidateEmail_Should_HaveError_When_EmailFormatIsInvalid()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Email = "invalid-email",
            Password = "password123"
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(RegisterUserRequestValidator.InvalidEmail);
    }

    [Test]
    public async Task ValidateEmail_Should_HaveError_When_EmailAlreadyExists()
    {
        // Arrange
        var existingEmail = "existing@example.com";

        await _dbContext.Users.AddAsync(new User
        {
            Email = existingEmail,
            Password = "password123"
        });

        await _dbContext.SaveChangesAsync();

        var request = new RegisterUserRequest
        {
            Email = existingEmail,
            Password = "password123"
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(RegisterUserRequestValidator.EmailAlreadyExists);
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task ValidatePassword_Should_HaveError_When_PasswordIsNullOrEmpty(string? password)
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Email = "test@example.com",
            Password = password
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(string.Format(ErrorMessages.REQUIRED, nameof(RegisterUserRequest.Password)));
    }
}