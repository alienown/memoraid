using FluentValidation;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IFlashcardService
{
    Task CreateFlashcardsAsync(CreateFlashcardsRequest request);
    Task<Response<GetFlashcardsResponse>> GetFlashcardsAsync(GetFlashcardsRequest request);
    Task<Response> DeleteFlashcardAsync(long id);
    Task<Response> UpdateFlashcardAsync(long id, UpdateFlashcardRequest request);

    public static class ErrorCodes
    {
        public const string FlashcardNotFound = "FlashcardNotFound";
    }
}

internal class FlashcardService : IFlashcardService
{
    private readonly IUserContext _userContext;
    private readonly MemoraidDbContext _dbContext;
    private readonly IFlashcardGenerationService _flashcardGenerationService;
    private readonly IValidator<CreateFlashcardsRequest> _createFlashcardsRequestValidator;
    private readonly IValidator<GetFlashcardsRequest>? _getFlashcardsRequestValidator;
    private readonly IValidator<long>? _deleteFlashcardRequestValidator;
    private readonly IValidator<UpdateFlashcardRequest>? _updateFlashcardRequestValidator;

    internal const string FlashcardNotFoundMessage = "Flashcard not found.";

    public FlashcardService(
        IUserContext userContext,
        MemoraidDbContext dbContext,
        IValidator<CreateFlashcardsRequest> createFlashcardsRequestValidator,
        IValidator<GetFlashcardsRequest> getFlashcardsValidator,
        IValidator<long>? deleteFlashcardValidator,
        IValidator<UpdateFlashcardRequest>? updateFlashcardValidator,
        IFlashcardGenerationService flashcardGenerationService)
    {
        _dbContext = dbContext;
        _createFlashcardsRequestValidator = createFlashcardsRequestValidator;
        _getFlashcardsRequestValidator = getFlashcardsValidator;
        _deleteFlashcardRequestValidator = deleteFlashcardValidator;
        _updateFlashcardRequestValidator = updateFlashcardValidator;
        _flashcardGenerationService = flashcardGenerationService;
        _userContext = userContext;
    }

    public async Task CreateFlashcardsAsync(CreateFlashcardsRequest request)
    {
        await _createFlashcardsRequestValidator.ValidateAndThrowAsync(request);

        var userId = _userContext.GetUserIdOrThrow();

        var flashcards = request.Flashcards!.Select(f => new Flashcard
        {
            UserId = userId,
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
            .Distinct()
            .ToList();

        if (generationIds.Count > 0)
        {
            await _flashcardGenerationService.UpdateGenerationMetricsAsync(generationIds);
        }

        await transaction.CommitAsync();
    }

    public async Task<Response<GetFlashcardsResponse>> GetFlashcardsAsync(GetFlashcardsRequest request)
    {
        _getFlashcardsRequestValidator.ValidateAndThrow(request);

        int pageNumber = request.PageNumber ?? 1;
        int pageSize = request.PageSize ?? 10;
        var userId = _userContext.GetUserIdOrThrow();

        var query = _dbContext.Flashcards
            .Where(f => f.UserId == userId)
            .AsNoTracking();

        int totalCount = await query.CountAsync();

        var flashcards = await query
            .OrderByDescending(f => f.CreatedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(f => new GetFlashcardsResponse.FlashcardsListItem(
                f.Id,
                f.Front,
                f.Back))
            .ToListAsync();

        var response = new GetFlashcardsResponse(flashcards, totalCount);

        return new Response<GetFlashcardsResponse>(response);
    }

    public async Task<Response> DeleteFlashcardAsync(long id)
    {
        _deleteFlashcardRequestValidator.ValidateAndThrow(id);

        var userId = _userContext.GetUserIdOrThrow();

        var flashcard = await _dbContext.Flashcards
            .Where(f => f.Id == id && f.UserId == userId)
            .SingleOrDefaultAsync();

        if (flashcard == null)
        {
            return new Response([new Response.Error(IFlashcardService.ErrorCodes.FlashcardNotFound, FlashcardNotFoundMessage, nameof(id))]);
        }

        _dbContext.Flashcards.Remove(flashcard);

        await _dbContext.SaveChangesAsync();

        return new Response();
    }

    public async Task<Response> UpdateFlashcardAsync(long id, UpdateFlashcardRequest request)
    {
        _updateFlashcardRequestValidator.ValidateAndThrow(request);

        var userId = _userContext.GetUserIdOrThrow();

        var flashcard = await _dbContext.Flashcards
            .Where(f => f.Id == id && f.UserId == userId)
            .SingleOrDefaultAsync();

        if (flashcard == null)
        {
            return new Response([new Response.Error(IFlashcardService.ErrorCodes.FlashcardNotFound, FlashcardNotFoundMessage, nameof(id))]);
        }

        flashcard.Front = request.Front!;
        flashcard.Back = request.Back!;

        await _dbContext.SaveChangesAsync();

        return new Response();
    }
}
