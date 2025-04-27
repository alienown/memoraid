using System.Collections.Generic;

namespace Memoraid.WebApi.Responses;

public class GenerateFlashcardsResponse
{
    public IReadOnlyList<GeneratedFlashcard> Flashcards { get; }
    public long GenerationId { get; }

    public GenerateFlashcardsResponse(IReadOnlyList<GeneratedFlashcard> flashcards, long generationId)
    {
        Flashcards = flashcards;
        GenerationId = generationId;
    }

    public class GeneratedFlashcard
    {
        public string Front { get; }
        public string Back { get; }

        public GeneratedFlashcard(string front, string back)
        {
            Front = front;
            Back = back;
        }
    }
}
