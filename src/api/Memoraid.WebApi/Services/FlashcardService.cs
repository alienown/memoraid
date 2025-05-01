using FluentValidation;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Requests;
using System.Linq;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IFlashcardService
{
    Task CreateFlashcardsAsync(CreateFlashcardsRequest request);
}

public class FlashcardService : IFlashcardService
{
    private readonly MemoraidDbContext _dbContext;
    private readonly IValidator<CreateFlashcardsRequest> _validator;
    private readonly IFlashcardGenerationService _flashcardGenerationService;

    public FlashcardService(
        MemoraidDbContext dbContext,
        IValidator<CreateFlashcardsRequest> validator,
        IFlashcardGenerationService flashcardGenerationService)
    {
        _dbContext = dbContext;
        _validator = validator;
        _flashcardGenerationService = flashcardGenerationService;
    }

    public async Task CreateFlashcardsAsync(CreateFlashcardsRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);

        var flashcards = request.Flashcards!.Select(f => new Flashcard
        {
            UserId = 1, // Assume user ID is 1 for now
            Front = f.Front!,
            Back = f.Back!,
            Source = f.Source!.Value,
            FlashcardAIGenerationId = f.GenerationId
        }).ToList();

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        await _dbContext.Flashcards.AddRangeAsync(flashcards);
        await _dbContext.SaveChangesAsync();

        var generationIds = flashcards
            .Where(f => f.FlashcardAIGenerationId.HasValue)
            .Select(f => f.FlashcardAIGenerationId!.Value)
            .ToList();

        if (generationIds.Count > 0)
        {
            await _flashcardGenerationService.UpdateGenerationMetricsAsync(generationIds);
        }

        await transaction.CommitAsync();
    }
}
