using FluentValidation;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IUserService
{
    Task<Response> RegisterUserAsync(RegisterUserRequest request);
}

public class UserService : IUserService
{
    private readonly MemoraidDbContext _dbContext;
    private readonly IValidator<RegisterUserRequest> _registerUserValidator;

    public UserService(MemoraidDbContext dbContext, IValidator<RegisterUserRequest> registerUserValidator)
    {
        _dbContext = dbContext;
        _registerUserValidator = registerUserValidator;
    }

    public async Task<Response> RegisterUserAsync(RegisterUserRequest request)
    {
        await _registerUserValidator.ValidateAndThrowAsync(request);

        var user = new User
        {
            Email = request.Email!,
            Password = HashPassword(request.Password!)
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return new Response();
    }

    private string HashPassword(string password)
    {
        var salt = new byte[16];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        return $"{Convert.ToBase64String(salt)}:{hashed}";
    }
}