public class CreateFlashcardsRequest
{
    public IReadOnlyList<Flashcard>? Flashcards { get; set; }

    public class Flashcard
    {
        public string? Front { get; set; }
        public string? Back { get; set; }
        public string? Source { get; set; }
        public long? GenerationId { get; set; }
    }
}

