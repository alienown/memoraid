using Memoraid.WebApi.Persistence.Enums;
using System.Collections.Generic;

namespace Memoraid.WebApi.Requests;

public class CreateFlashcardsRequest
{
    public IReadOnlyList<CreateFlashcardData>? Flashcards { get; set; }

    public class CreateFlashcardData
    {
        public string? Front { get; set; }
        public string? Back { get; set; }
        public FlashcardSource? Source { get; set; }
        public long? GenerationId { get; set; }
    }
}