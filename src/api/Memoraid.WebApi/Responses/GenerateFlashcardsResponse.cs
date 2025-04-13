public class GenerateFlashcardsResponse
{
    public IReadOnlyList<Flashcard> Flashcards { get; }
    public int GenerationId { get; }

    public GenerateFlashcardsResponse(IReadOnlyList<Flashcard> flashcards, int generationId)
    {
        Flashcards = flashcards;
        GenerationId = generationId;
    }

    public class Flashcard
    {
        public string Front { get; }
        public string Back { get; }

        public Flashcard(string front, string back)
        {
            Front = front;
            Back = back;
        }
    }
}
