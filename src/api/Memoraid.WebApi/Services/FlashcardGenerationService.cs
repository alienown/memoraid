using FluentValidation;
using FluentValidation.Results;
using Memoraid.WebApi.Persistence;
using Memoraid.WebApi.Persistence.Entities;
using Memoraid.WebApi.Persistence.Enums;
using Memoraid.WebApi.Requests;
using Memoraid.WebApi.Responses;
using Memoraid.WebApi.Services.OpenRouter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services;

public interface IFlashcardGenerationService
{
    Task<GenerateFlashcardsResponse> GenerateFlashcardsAsync(GenerateFlashcardsRequest request);
    Task UpdateGenerationMetricsAsync(IEnumerable<long> generationIds);
}

internal class FlashcardGenerationService : IFlashcardGenerationService
{
    internal const string AIModel = "openai/gpt-4o-mini";
    internal const string GenerationPrompt = """
        You are an expert educator specializing in creating effective flashcards for learning.
    
        Guidelines for creating high-quality flashcards:
        - Create concise, clear flashcards from the provided text enclosed in <sourceText></sourceText> tags
        - Do not use your internal knowledge or external sources for creating flashcards. Use only the provided <sourceText> and nothing else
        - Each flashcard should have a question or concept on the front and a brief, accurate answer on the back
        - Front side should contain a specific question, term, concept, or prompt that tests recall
        - Back side should provide a concise, complete answer (preferably 1-3 sentences)
        - Focus on key concepts, definitions, facts, and relationships from the source text
        - Use simple, clear language and avoid overly complex terminology unless essential
        - For complex topics, break them down into multiple focused flashcards
        - Flashcards must be created in the same language as the source text
        """;

    private readonly IUserContext _userContext;
    private readonly MemoraidDbContext _dbContext;
    private readonly IOpenRouterService _openRouterService;
    private readonly IValidator<GenerateFlashcardsRequest> _generateFlashcardsRequestValidator;

    public FlashcardGenerationService(
        IUserContext userContext,
        MemoraidDbContext dbContext,
        IOpenRouterService openRouterService,
        IValidator<GenerateFlashcardsRequest> generateFlashcardsRequestValidator)
    {
        _userContext = userContext;
        _dbContext = dbContext;
        _openRouterService = openRouterService;
        _generateFlashcardsRequestValidator = generateFlashcardsRequestValidator;
    }

    public async Task<GenerateFlashcardsResponse> GenerateFlashcardsAsync(GenerateFlashcardsRequest request)
    {
        _generateFlashcardsRequestValidator.ValidateAndThrow(request);

        var flashcards = await GenerateFlashcardsUsingAIAsync(request.SourceText!);
        var userId = _userContext.GetUserIdOrThrow();

        var generationRecord = new FlashcardAIGeneration
        {
            UserId = userId,
            AIModel = AIModel,
            SourceText = request.SourceText!,
            AllFlashcardsCount = flashcards.Count,
        };

        await _dbContext.FlashcardAIGenerations.AddAsync(generationRecord);
        await _dbContext.SaveChangesAsync();

        return new GenerateFlashcardsResponse(flashcards, generationRecord.Id);
    }

    private async Task<IReadOnlyList<GenerateFlashcardsResponse.GeneratedFlashcard>> GenerateFlashcardsUsingAIAsync(string sourceText)
    {
        var schema = JsonDocument.Parse(@"{
            ""type"": ""object"",
            ""properties"": {
                ""flashcards"": {
                    ""type"": ""array"",
                    ""items"": {
                        ""type"": ""object"",
                        ""properties"": {
                            ""front"": { ""type"": ""string"" },
                            ""back"": { ""type"": ""string"" }
                        },
                        ""required"": [""front"", ""back""],
                        ""additionalProperties"": false
                    }
                }
            },
            ""required"": [""flashcards""],
            ""additionalProperties"": false
        }");

        var request = new CompleteWithStructuredOutputRequest
        {
            Model = AIModel,
            Messages =
            [
                new()
                {
                    Role = ChatRole.System,
                    Content = GenerationPrompt
                },
                new()
                {
                    Role = ChatRole.User,
                    Content = $"<sourceText>{sourceText}</sourceText>"
                }
            ],
            JsonSchema = new()
            {
                Name = "flashcardGeneration",
                Schema = schema.RootElement
            }
        };

        try
        {
            var result = await _openRouterService.CompleteWithStructuredOutputAsync<FlashcardGenerationResult>(request);

            CutStringLengthsIfExceedLimits(result);

            return [.. result.Flashcards.Select(f => new GenerateFlashcardsResponse.GeneratedFlashcard(f.Front, f.Back))];
        }
        catch (Exception)
        {
            throw new ValidationException(
                "Failed to generate flashcards",
                [new ValidationFailure() { ErrorMessage = "Failed to generate flashcards" }]);
        }
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

    private void CutStringLengthsIfExceedLimits(FlashcardGenerationResult result)
    {
        foreach (var flashcard in result.Flashcards)
        {
            if (flashcard.Front.Length > 500)
            {
                flashcard.Front = flashcard.Front[..500];
            }

            if (flashcard.Back.Length > 200)
            {
                flashcard.Back = flashcard.Back[..200];
            }
        }
    }

    internal class FlashcardGenerationResult
    {
        public required List<Flashcard> Flashcards { get; set; }

        public class Flashcard
        {
            public required string Front { get; set; }
            public required string Back { get; set; }
        }
    }
}