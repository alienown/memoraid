using FluentValidation;
using Memoraid.WebApi.Constants;
using Memoraid.WebApi.Requests;

namespace Memoraid.WebApi.Validation;

public class GetFlashcardsRequestValidator : AbstractValidator<GetFlashcardsRequest>
{
    public GetFlashcardsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .When(x => x.PageNumber.HasValue)
            .WithMessage(string.Format(ErrorMessages.GREATER_THAN, nameof(GetFlashcardsRequest.PageNumber), 0));

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .When(x => x.PageSize.HasValue)
            .WithMessage(string.Format(ErrorMessages.GREATER_THAN, nameof(GetFlashcardsRequest.PageSize), 0));
    }
}