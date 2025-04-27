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

    public FlashcardService(
        MemoraidDbContext dbContext,
        IValidator<CreateFlashcardsRequest> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
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

        await _dbContext.Flashcards.AddRangeAsync(flashcards);
        await _dbContext.SaveChangesAsync();
    }
}
