using FluentValidation;
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
public class UserServiceTests
{
    private MemoraidDbContext _dbContext = null!;
    private RegisterUserRequestValidator _validator = null!;
    private UserService _userService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor())
            .Options;

        _dbContext = new MemoraidDbContext(options);
        _validator = new RegisterUserRequestValidator(_dbContext);
        _userService = new UserService(_dbContext, _validator);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task RegisterUserAsync_Should_RegisterUser_When_RequestIsValid()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        // Act
        var result = await _userService.RegisterUserAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var savedUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
        savedUser.ShouldNotBeNull();
        savedUser.Email.ShouldBe(request.Email);
        savedUser.Password.ShouldNotBe(request.Password);
        savedUser.Password.ShouldContain(":");
    }

    [Test]
    public async Task RegisterUserAsync_Should_ThrowValidationException_When_EmailIsInvalid()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Email = "invalid-email",
            Password = "Password123!"
        };

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(async () =>
            await _userService.RegisterUserAsync(request));

        exception.Errors.Count().ShouldBe(1);
        exception.Errors.ShouldContain(e => e.ErrorMessage == RegisterUserRequestValidator.InvalidEmail);
    }
}
