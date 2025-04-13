public class GetFlashcardsResponse
{
    public GetFlashcardsResponse(IReadOnlyList<Flashcard> items, int total)
    {
        Items = items;
        Total = total;
    }

    public IReadOnlyList<Flashcard> Items { get; }
    public int Total { get; }

    public class Flashcard
    {
        public Flashcard(int id, string front, string back)
        {
            Id = id;
            Front = front;
            Back = back;
        }

        public int Id { get; }
        public string Front { get; }
        public string Back { get; }
    }
}
