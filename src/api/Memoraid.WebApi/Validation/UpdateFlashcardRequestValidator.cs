using FluentValidation;
using Memoraid.WebApi.Requests;
using static Memoraid.WebApi.Constants.ErrorMessages;

namespace Memoraid.WebApi.Validation;

public class UpdateFlashcardRequestValidator : AbstractValidator<UpdateFlashcardRequest>
{
    public UpdateFlashcardRequestValidator()
    {
        RuleFor(x => x.Front)
            .NotEmpty()
            .WithMessage(string.Format(REQUIRED, nameof(UpdateFlashcardRequest.Front)))
            .MaximumLength(500)
            .WithMessage(string.Format(MAX_LENGTH, nameof(UpdateFlashcardRequest.Front), 500));

        RuleFor(x => x.Back)
            .NotEmpty()
            .WithMessage(string.Format(REQUIRED, nameof(UpdateFlashcardRequest.Back)))
            .MaximumLength(200)
            .WithMessage(string.Format(MAX_LENGTH, nameof(UpdateFlashcardRequest.Back), 200));
    }
}