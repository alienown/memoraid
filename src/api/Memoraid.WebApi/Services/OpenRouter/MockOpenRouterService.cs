using Bogus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoraid.WebApi.Services.OpenRouter;

internal class MockOpenRouterService : IOpenRouterService
{
    private readonly Random _random = new();
    private readonly Faker _faker = new();

    public Task<T> CompleteWithStructuredOutputAsync<T>(CompleteWithStructuredOutputRequest request)
    {
        if (typeof(T).Name == "FlashcardGenerationResult")
        {
            var count = _random.Next(3, 9);

            var flashcards = new List<FlashcardGenerationService.FlashcardGenerationResult.Flashcard>();

            for (int i = 0; i < count; i++)
            {
                flashcards.Add(new FlashcardGenerationService.FlashcardGenerationResult.Flashcard
                {
                    Front = GenerateFlashcardFront(),
                    Back = GenerateFlashcardBack()
                });
            }

            var result = new FlashcardGenerationService.FlashcardGenerationResult
            {
                Flashcards = flashcards
            };

            return Task.FromResult((T)(object)result);
        }

        throw new NotImplementedException($"Mock generation for type {typeof(T).Name} is not implemented");
    }

    private string GenerateFlashcardFront()
    {
        var questionTypes = new List<Func<string>>
        {
            () => $"What is {_faker.Commerce.ProductName()}?",
            () => $"Define {_faker.Music}:",
            () => $"Explain the concept of {_faker.Hacker.Noun()}:",
            () => $"How does {_faker.Commerce.ProductAdjective()} {_faker.Hacker.Verb()} work?",
            () => $"What are the key characteristics of {_faker.Music}?",
            () => $"Compare and contrast {_faker.Hacker.Abbreviation()} and {_faker.Hacker.Abbreviation()}:"
        };

        var index = _random.Next(questionTypes.Count);

        return questionTypes[index]();
    }

    private string GenerateFlashcardBack()
    {
        return _faker.Lorem.Paragraph(1);
    }
}
