using FluentValidation;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Requests;

namespace Memoraid.WebApi.Validation;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    internal const string InvalidEmail = "Invalid email address";

    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.REQUIRED, nameof(LoginUserRequest.Email)))
            .EmailAddress()
            .WithMessage(InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.REQUIRED, nameof(LoginUserRequest.Password)));
    }
}