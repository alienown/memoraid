using FluentValidation;

namespace Memoraid.WebApi.Validation;

public class DeleteFlashcardRequestValidator : AbstractValidator<long>
{
    internal const string InvalidFlashcardIdMessage = "Flashcard ID must be greater than zero.";

    public DeleteFlashcardRequestValidator()
    {
        RuleFor(id => id)
            .GreaterThan(0)
            .WithMessage(InvalidFlashcardIdMessage);
    }
}