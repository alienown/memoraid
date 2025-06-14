import {
  FlashcardListItem,
  FlashcardListItemSkeleton,
} from "./FlashcardListItem";
import { FlashcardData } from "./types";

export interface FlashcardListProps {
  flashcards: FlashcardData[];
  disabled: boolean;
  onAccept: (index: number) => void;
  onReject: (index: number) => void;
  onEdit: (index: number) => void;
}

export function FlashcardList({
  flashcards,
  disabled,
  onAccept,
  onReject,
  onEdit,
}: FlashcardListProps) {
  if (flashcards.length === 0) {
    return null;
  }

  return (
    <div className="space-y-4">
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        {flashcards.map((card, index) => (
          <FlashcardListItem
            key={index}
            front={card.front}
            back={card.back}
            isAccepted={card.isAccepted}
            disabled={disabled}
            onAccept={() => onAccept(index)}
            onReject={() => onReject(index)}
            onEdit={() => onEdit(index)}
          />
        ))}
      </div>
    </div>
  );
}

export function FlashcardsListSkeleton({ count = 6 }: { count?: number }) {
  return (
    <div className="space-y-4">
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        {Array.from({ length: count }).map((_, index) => (
          <FlashcardListItemSkeleton key={index} />
        ))}
      </div>
    </div>
  );
}
