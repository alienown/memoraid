using FluentValidation;
using Memoraid.WebApi.Constants;

namespace Memoraid.WebApi.Validation;

public class DeleteFlashcardRequestValidator : AbstractValidator<long>
{
    public DeleteFlashcardRequestValidator()
    {
        RuleFor(id => id)
            .GreaterThan(0)
            .WithMessage(string.Format(ErrorMessages.GREATER_THAN, "id", 0));
    }
}