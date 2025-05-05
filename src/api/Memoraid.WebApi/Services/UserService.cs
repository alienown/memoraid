using FluentValidation;
using Memoraid.WebApi.Configuration;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IUserService
{
    Task<Response> RegisterUserAsync(RegisterUserRequest request);
    Task<Response<LoginUserResponse>> LoginUserAsync(LoginUserRequest request);

    public static class ErrorCodes
    {
        public const string InvalidCredentials = "InvalidCredentials";
    }
}

public class UserService : IUserService
{
    private readonly MemoraidDbContext _dbContext;
    private readonly IValidator<RegisterUserRequest> _registerUserValidator;
    private readonly IValidator<LoginUserRequest> _loginUserValidator;
    private readonly ApplicationOptions _appOptions;

    public UserService(
        MemoraidDbContext dbContext,
        IValidator<RegisterUserRequest> registerUserValidator,
        IValidator<LoginUserRequest> loginUserValidator,
        IOptions<ApplicationOptions> appOptions)
    {
        _dbContext = dbContext;
        _registerUserValidator = registerUserValidator;
        _loginUserValidator = loginUserValidator;
        _appOptions = appOptions.Value;
    }

    public async Task<Response> RegisterUserAsync(RegisterUserRequest request)
    {
        await _registerUserValidator.ValidateAndThrowAsync(request);

        var user = new User
        {
            Email = request.Email!,
            Password = GeneratePasswordHash(request.Password!)
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return new Response();
    }

    public async Task<Response<LoginUserResponse>> LoginUserAsync(LoginUserRequest request)
    {
        await _loginUserValidator.ValidateAndThrowAsync(request);

        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !VerifyPassword(request.Password!, user.Password))
        {
            return new Response<LoginUserResponse>([new Response.Error(IUserService.ErrorCodes.InvalidCredentials, "Invalid credentials")]);
        }

        var token = GenerateJwtToken(user);

        return new Response<LoginUserResponse>(new LoginUserResponse(token));
    }

    private string GeneratePasswordHash(string password)
    {
        var salt = new byte[16];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hashed = HashPassword(password, salt);

        return $"{Convert.ToBase64String(salt)}:{hashed}";
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(':');

        if (parts.Length != 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var storedPasswordHash = parts[1];

        var computedHash = HashPassword(password, salt);

        return computedHash == storedPasswordHash;
    }

    private string HashPassword(string password, byte[] salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_appOptions.Jwt.Secret);

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("sub", user.Id.ToString()),
                new Claim("iss", _appOptions.Jwt.Issuer),
                new Claim("aud", _appOptions.Jwt.Audience),
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}