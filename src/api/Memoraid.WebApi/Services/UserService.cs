using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Responses;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

internal class UserService : IUserService
{
    private readonly IUserContext _userContext;
    private readonly MemoraidDbContext _dbContext;
    private readonly IFlashcardService _flashcardService;
    private readonly IFlashcardGenerationService _flashcardGenerationService;

    internal const string UserNotFoundMessage = "User not found.";

    public UserService(
        IUserContext userContext,
        MemoraidDbContext dbContext,
        IFlashcardService flashcardService,
        IFlashcardGenerationService flashcardGenerationService)
    {
        _userContext = userContext;
        _dbContext = dbContext;
        _flashcardService = flashcardService;
        _flashcardGenerationService = flashcardGenerationService;
    }

    public async Task<Response> DeleteUserAsync()
    {
        var userId = _userContext.GetUserIdOrThrow();
        
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        await _flashcardService.DeleteUserFlashcardsAsync(userId);
        await _flashcardGenerationService.DeleteUserFlashcardGenerationsAsync(userId);

        await transaction.CommitAsync();

        return new Response();
    }
}
