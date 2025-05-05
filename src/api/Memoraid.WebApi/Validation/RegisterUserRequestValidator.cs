using FluentValidation;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Requests;
using Microsoft.EntityFrameworkCore;

namespace Memoraid.WebApi.Validation;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    internal const string InvalidEmail = "Invalid email address";
    internal const string EmailAlreadyExists = "A user with this email already exists";

    public RegisterUserRequestValidator(MemoraidDbContext dbContext)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.REQUIRED, nameof(RegisterUserRequest.Email)))
            .EmailAddress()
            .WithMessage(InvalidEmail)
            .MustAsync(async (email, cancellation) =>
            {
                var exists = await dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email, cancellation);

                return !exists;
            })
            .WithMessage(EmailAlreadyExists);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.REQUIRED, nameof(RegisterUserRequest.Password)));
    }
}