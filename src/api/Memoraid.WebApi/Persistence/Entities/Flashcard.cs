using Memoraid.WebApi.Persistence.Enums;

namespace Memoraid.WebApi.Persistence.Entities
{
    public class Flashcard : EntityBase<long>
    {
        public long UserId { get; set; }
        public long? FlashcardAIGenerationId { get; set; }
        public required string Front { get; set; }
        public required string Back { get; set; }
        public FlashcardSource Source { get; set; }
    }
}