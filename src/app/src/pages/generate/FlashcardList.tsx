import { FlashcardListItem } from "./FlashcardListItem";
import { FlashcardData } from "./types";

export interface FlashcardListProps {
  flashcards: FlashcardData[];
  onAccept: (index: number) => void;
  onReject: (index: number) => void;
  onEdit: (index: number) => void;
}

export function FlashcardList({
  flashcards,
  onAccept,
  onReject,
  onEdit,
}: FlashcardListProps) {
  if (flashcards.length === 0) {
    return null;
  }

  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold mb-4">
        Generated Flashcards ({flashcards.length})
      </h2>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        {flashcards.map((card, index) => (
          <FlashcardListItem
            key={index}
            front={card.front}
            back={card.back}
            isAccepted={card.isAccepted}
            onAccept={() => onAccept(index)}
            onReject={() => onReject(index)}
            onEdit={() => onEdit(index)}
          />
        ))}
      </div>
    </div>
  );
}
