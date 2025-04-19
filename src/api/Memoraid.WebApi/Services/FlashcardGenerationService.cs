using FluentValidation;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Memoraid.WebApi.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IFlashcardGenerationService
{
    Task<GenerateFlashcardsResponse> GenerateFlashcardsAsync(GenerateFlashcardsRequest request);
}

public class FlashcardGenerationService : IFlashcardGenerationService
{
    private readonly GenerateFlashcardsRequestValidator _validator = new();
    private readonly Random _rng = new();
    private readonly MemoraidDbContext _dbContext;

    public FlashcardGenerationService(MemoraidDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GenerateFlashcardsResponse> GenerateFlashcardsAsync(GenerateFlashcardsRequest request)
    {
        _validator.ValidateAndThrow(request);

        var flashcards = GenerateRandomFlashcards();

        var generationRecord = new FlashcardAIGeneration
        {
            UserId = 1,
            AIModel = "Mock",
            SourceText = request.SourceText!,
            AllFlashcardsCount = flashcards.Count,
        };

        await _dbContext.FlashcardAIGenerations.AddAsync(generationRecord);
        await _dbContext.SaveChangesAsync();

        return new GenerateFlashcardsResponse(flashcards, generationRecord.Id);
    }

    private IReadOnlyList<GenerateFlashcardsResponse.Flashcard> GenerateRandomFlashcards()
    {
        var count = _rng.Next(3, 7);

        return Enumerable.Range(1, count)
            .Select(i => new GenerateFlashcardsResponse.Flashcard($"Front mock {i}", $"Back mock {i}"))
            .ToList();
    }
}