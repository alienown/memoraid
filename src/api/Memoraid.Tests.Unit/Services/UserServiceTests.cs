using FluentValidation;
using Memoraid.WebApi.Configuration;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Interceptors;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Services;
using Memoraid.WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Memoraid.Tests.Unit.Services;

[TestFixture]
public class UserServiceTests
{
    private MemoraidDbContext _dbContext = null!;
    private RegisterUserRequestValidator _registerUserValidator = null!;
    private LoginUserRequestValidator _loginUserValidator = null!;
    private UserService _userService = null!;
    private IOptions<ApplicationOptions> _appOptions = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MemoraidDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(new SaveEntityBaseInterceptor())
            .Options;

        _dbContext = new MemoraidDbContext(options);
        _registerUserValidator = new RegisterUserRequestValidator(_dbContext);
        _loginUserValidator = new LoginUserRequestValidator();
        
        // Configure application options with test JWT settings
        var appOptions = new ApplicationOptions
        {
            Jwt = new Jwt
            {
                Secret = "test_secret_key_for_jwt_with_minimum_length_of_32_chars",
                Issuer = "test_issuer",
                Audience = "test_audience"
            },
            OpenRouter = new OpenRouter
            {
                ApiKey = "",
                ApiBaseUrl = ""
            }
        };
        
        _appOptions = Options.Create(appOptions);
        
        _userService = new UserService(
            _dbContext, 
            _registerUserValidator, 
            _loginUserValidator, 
            _appOptions);
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

    [Test]
    public async Task LoginUserAsync_Should_ReturnToken_When_CredentialsAreValid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "Password123!";

        // Register a user first
        var registerRequest = new RegisterUserRequest
        {
            Email = email,
            Password = password
        };
        await _userService.RegisterUserAsync(registerRequest);

        var loginRequest = new LoginUserRequest
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _userService.LoginUserAsync(loginRequest);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Token.ShouldNotBeNullOrEmpty();
    }

    [Test]
    public async Task LoginUserAsync_Should_ReturnError_When_UserDoesNotExist()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginRequest);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.Count().ShouldBe(1);
        result.Errors.ShouldContain(e => e.Code == IUserService.ErrorCodes.InvalidCredentials);
        result.Data.ShouldBeNull();
    }

    [Test]
    public async Task LoginUserAsync_Should_ReturnError_When_PasswordIsIncorrect()
    {
        // Arrange
        var email = "test@example.com";
        var password = "Password123!";

        // Register a user first
        var registerRequest = new RegisterUserRequest
        {
            Email = email,
            Password = password
        };
        await _userService.RegisterUserAsync(registerRequest);

        var loginRequest = new LoginUserRequest
        {
            Email = email,
            Password = "WrongPassword!"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginRequest);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Errors.Count().ShouldBe(1);
        result.Errors.ShouldContain(e => e.Code == IUserService.ErrorCodes.InvalidCredentials);
        result.Data.ShouldBeNull();
    }

    [Test]
    public async Task LoginUserAsync_Should_ThrowValidationException_When_EmailIsInvalid()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "invalid-email",
            Password = "Password123!"
        };

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(async () =>
            await _userService.LoginUserAsync(loginRequest));

        exception.Errors.Count().ShouldBe(1);
        exception.Errors.ShouldContain(e => e.ErrorMessage == LoginUserRequestValidator.InvalidEmail);
    }

    [Test]
    public async Task LoginUserAsync_Should_ReturnTokenWithCorrectPayload_When_CredentialsAreValid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "Password123!";

        // Register a user first
        var registerRequest = new RegisterUserRequest
        {
            Email = email,
            Password = password
        };
        await _userService.RegisterUserAsync(registerRequest);

        var user = await _dbContext.Users.SingleAsync(u => u.Email == email);
        
        var loginRequest = new LoginUserRequest
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _userService.LoginUserAsync(loginRequest);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Token.ShouldNotBeNullOrEmpty();
        
        // Verify token format and payload
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.CanReadToken(result.Data.Token).ShouldBeTrue();
        
        var jwtToken = tokenHandler.ReadJwtToken(result.Data.Token);
        
        // Verify token has required claims
        jwtToken.Claims.ShouldContain(c => c.Type == "sub" && c.Value == user.Id.ToString());
        jwtToken.Claims.ShouldContain(c => c.Type == "iss" && c.Value == _appOptions.Value.Jwt.Issuer);
        jwtToken.Claims.ShouldContain(c => c.Type == "aud" && c.Value == _appOptions.Value.Jwt.Audience);
        
        // Verify expiration time is set correctly - should be around 1 hour in the future
        var expectedExpiration = DateTime.UtcNow.AddHours(1);
        var actualExpiration = jwtToken.ValidTo;
        
        // Allow small time deviation (few seconds) between test execution and token generation
        Math.Abs((actualExpiration - expectedExpiration).TotalMinutes).ShouldBeLessThan(1);
        
        // Verify algorithm is correct
        jwtToken.SignatureAlgorithm.ShouldBe("HS256");
    }
}
