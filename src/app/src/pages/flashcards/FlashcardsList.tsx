import {
  FlashcardListItem,
  FlashcardListItemSkeleton,
} from "./FlashcardListItem";

export interface FlashcardsListProps {
  flashcards: Array<{ id: number; front: string; back: string }>;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
}

export function FlashcardsList({
  flashcards,
  onEdit,
  onDelete,
}: FlashcardsListProps) {
  if (flashcards.length === 0) {
    return null;
  }

  return (
    <div className="space-y-4">
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        {flashcards.map((card) => (
          <FlashcardListItem
            key={card.id}
            id={card.id}
            front={card.front}
            back={card.back}
            onEdit={() => onEdit(card.id)}
            onDelete={() => onDelete(card.id)}
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
