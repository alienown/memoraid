using FluentValidation;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Memoraid.WebApi.Constants.ErrorMessages;

namespace Memoraid.WebApi.Validation;

public class CreateFlashcardsRequestValidator : AbstractValidator<CreateFlashcardsRequest>
{
    internal const string FlashcardsAreRequired = "At least one flashcard must be provided.";
    internal const string InvalidSourceError = "Source must be one of: Manual, AIFull, or AIEdited.";
    internal const string GenerationIdMustBeNullForFlashcardsWithManualSource = "GenerationId must be null for manually created flashcards.";
    internal const string GenerationIdMustBeGreaterThanZeroError = "GenerationId must be greater than zero.";
    internal const string GenerationIdsForAIGeneratedFlashcardsMustBeTheSame = "All AI-generated flashcards must have the same generationId.";
    internal const string GenerationNotExistsError = "The specified AI generation does not exist.";

    private readonly MemoraidDbContext _dbContext;

    public CreateFlashcardsRequestValidator(MemoraidDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Flashcards)
            .NotEmpty()
            .WithMessage(FlashcardsAreRequired);

        RuleForEach(x => x.Flashcards)
            .SetValidator(new FlashcardValidator());

        RuleFor(x => x)
            .Custom(ValidateAIFlashcardsHaveSameGenerationId);

        RuleFor(x => x)
            .CustomAsync(ValidateAIFlashcardGenerationExistsAsync);
    }

    private void ValidateAIFlashcardsHaveSameGenerationId(CreateFlashcardsRequest request, ValidationContext<CreateFlashcardsRequest> context)
    {
        if (request.Flashcards == null || !request.Flashcards.Any())
            return;

        var aiFlashcards = request.Flashcards.Where(f =>
            f.Source == FlashcardSource.AIFull ||
            f.Source == FlashcardSource.AIEdited).ToList();

        if (!aiFlashcards.Any())
            return;

        var generationIds = aiFlashcards.Select(f => f.GenerationId).Distinct().ToList();

        if (generationIds.Count > 1)
        {
            context.AddFailure(nameof(request.Flashcards), GenerationIdsForAIGeneratedFlashcardsMustBeTheSame);
        }
    }

    private async Task ValidateAIFlashcardGenerationExistsAsync(
        CreateFlashcardsRequest request,
        ValidationContext<CreateFlashcardsRequest> context,
        CancellationToken cancellationToken)
    {
        if (request.Flashcards == null || !request.Flashcards.Any())
            return;

        var aiFlashcards = request.Flashcards.Where(f =>
            f.Source == FlashcardSource.AIFull ||
            f.Source == FlashcardSource.AIEdited).ToList();

        if (!aiFlashcards.Any())
            return;

        var generationId = aiFlashcards.First().GenerationId;

        if (!generationId.HasValue)
            return;

        var generationExists = await _dbContext.FlashcardAIGenerations
            .AnyAsync(g => g.Id == generationId && g.UserId == 1, cancellationToken);

        if (!generationExists)
        {
            context.AddFailure(nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId),
                GenerationNotExistsError);
        }
    }

    private class FlashcardValidator : AbstractValidator<CreateFlashcardsRequest.CreateFlashcardData>
    {
        public FlashcardValidator()
        {
            RuleFor(x => x.Front)
                .NotNull()
                .WithMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Front)))
                .MaximumLength(500)
                .WithMessage(string.Format(MAX_LENGTH, nameof(CreateFlashcardsRequest.CreateFlashcardData.Front), 500));

            RuleFor(x => x.Back)
                .NotNull()
                .WithMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Back)))
                .MaximumLength(200)
                .WithMessage(string.Format(MAX_LENGTH, nameof(CreateFlashcardsRequest.CreateFlashcardData.Back), 200));

            RuleFor(x => x.Source)
                .NotNull()
                .WithMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.Source)))
                .IsInEnum()
                .WithMessage(InvalidSourceError);

            When(x => x.Source == FlashcardSource.Manual, () =>
            {
                RuleFor(x => x.GenerationId)
                    .Null()
                    .WithMessage("GenerationId must be null for manually created flashcards.");
            });

            When(x => x.Source == FlashcardSource.AIFull || x.Source == FlashcardSource.AIEdited, () =>
            {
                RuleFor(x => x.GenerationId)
                    .NotNull().WithMessage(string.Format(REQUIRED, nameof(CreateFlashcardsRequest.CreateFlashcardData.GenerationId)))
                    .GreaterThan(0).WithMessage("GenerationId must be greater than zero.");
            });
        }
    }
}
