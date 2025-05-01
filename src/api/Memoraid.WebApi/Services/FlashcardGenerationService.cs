using FluentValidation;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IFlashcardGenerationService
{
    Task<GenerateFlashcardsResponse> GenerateFlashcardsAsync(GenerateFlashcardsRequest request);
    Task UpdateGenerationMetricsAsync(IEnumerable<long> generationIds);
}

public class FlashcardGenerationService : IFlashcardGenerationService
{
    private readonly Random _rng = new();
    private readonly MemoraidDbContext _dbContext;
    private readonly IValidator<GenerateFlashcardsRequest> _validator;

    public FlashcardGenerationService(MemoraidDbContext dbContext, IValidator<GenerateFlashcardsRequest> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
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

    private IReadOnlyList<GenerateFlashcardsResponse.GeneratedFlashcard> GenerateRandomFlashcards()
    {
        var count = _rng.Next(3, 7);

        return Enumerable.Range(1, count)
            .Select(i => new GenerateFlashcardsResponse.GeneratedFlashcard($"Front mock {i}", $"Back mock {i}"))
            .ToList();
    }

    public async Task UpdateGenerationMetricsAsync(IEnumerable<long> generationIds)
    {
        if (!generationIds.Any())
        {
            return;
        }

        var distinctGenerationIds = generationIds.Distinct().ToList();

        var generations = await _dbContext.FlashcardAIGenerations
            .Where(g => distinctGenerationIds.Contains(g.Id))
            .ToListAsync();

        if (!generations.Any())
        {
            return;
        }

        var allFlashcards = await _dbContext.Flashcards
            .Where(f => f.FlashcardAIGenerationId.HasValue && distinctGenerationIds.Contains(f.FlashcardAIGenerationId.Value))
            .ToListAsync();

        var flashcardsByGenerationId = allFlashcards.ToLookup(f => f.FlashcardAIGenerationId);

        foreach (var generation in generations)
        {
            var flashcards = flashcardsByGenerationId[generation.Id].ToList();

            generation.AllFlashcardsCount = flashcards.Count;

            generation.AcceptedUneditedFlashcardsCount =
                flashcards.Count(f => f.Source == FlashcardSource.AIFull);

            generation.AcceptedEditedFlashcardsCount =
                flashcards.Count(f => f.Source == FlashcardSource.AIEdited);
        }

        await _dbContext.SaveChangesAsync();
    }
}