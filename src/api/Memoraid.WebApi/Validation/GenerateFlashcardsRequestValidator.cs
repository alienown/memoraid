using FluentValidation;
using Memoraid.WebApi.Requests;
using static Memoraid.WebApi.Constants.ErrorMessages;

namespace Memoraid.WebApi.Validation;

public class GenerateFlashcardsRequestValidator : AbstractValidator<GenerateFlashcardsRequest>
{
    internal const string SourceTextFieldName = "Source text";

    public GenerateFlashcardsRequestValidator()
    {
        RuleFor(x => x.SourceText)
            .NotEmpty().WithMessage(string.Format(REQUIRED, SourceTextFieldName))
            .MaximumLength(10000).WithMessage(string.Format(MAX_LENGTH, SourceTextFieldName, 10000));
    }
}