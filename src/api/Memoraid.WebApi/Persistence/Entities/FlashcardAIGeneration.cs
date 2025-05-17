namespace Memoraid.WebApi.Persistence.Entities;

public class FlashcardAIGeneration : EntityBase<long>
{
    public required string UserId { get; set; }
    public required string AIModel { get; set; }
    public required string SourceText { get; set; }
    public int AllFlashcardsCount { get; set; }
    public int? AcceptedUneditedFlashcardsCount { get; set; }
    public int? AcceptedEditedFlashcardsCount { get; set; }
}