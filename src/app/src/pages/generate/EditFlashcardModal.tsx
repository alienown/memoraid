import { useState, useEffect } from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { FlashcardData } from "./types";

interface EditFlashcardModalProps {
  isOpen: boolean;
  flashcard: FlashcardData | null;
  onSave: (editedCard: { front: string; back: string }) => void;
  onCancel: () => void;
}

export function EditFlashcardModal({
  isOpen,
  flashcard,
  onSave,
  onCancel,
}: EditFlashcardModalProps) {
  const [front, setFront] = useState("");
  const [back, setBack] = useState("");
  const [frontError, setFrontError] = useState<string | null>(null);
  const [backError, setBackError] = useState<string | null>(null);

  // Reset state when the modal opens with a new flashcard
  useEffect(() => {
    if (flashcard) {
      setFront(flashcard.front);
      setBack(flashcard.back);
      setFrontError(null);
      setBackError(null);
    }
  }, [flashcard]);

  const validateForm = (): boolean => {
    let isValid = true;

    if (!front.trim()) {
      setFrontError("Front text is required");
      isValid = false;
    } else if (front.length > 500) {
      setFrontError("Front text cannot exceed 500 characters");
      isValid = false;
    } else {
      setFrontError(null);
    }

    if (!back.trim()) {
      setBackError("Back text is required");
      isValid = false;
    } else if (back.length > 200) {
      setBackError("Back text cannot exceed 200 characters");
      isValid = false;
    } else {
      setBackError(null);
    }

    return isValid;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (validateForm()) {
      onSave({ front: front.trim(), back: back.trim() });
    }
  };

  const handleFrontChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setFront(e.target.value);
    if (frontError) setFrontError(null);
  };

  const handleBackChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setBack(e.target.value);
    if (backError) setBackError(null);
  };

  return (
    <Dialog open={isOpen} onOpenChange={(open) => !open && onCancel()}>
      <DialogContent className="sm:max-w-[900px]">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Edit Flashcard</DialogTitle>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="space-y-2">
              <label htmlFor="front" className="text-sm font-medium">
                Front (Question)
              </label>
              <Textarea
                id="front"
                value={front}
                onChange={handleFrontChange}
                placeholder="Enter the question or term (max 500 characters)"
                className="resize-y h-30"
                maxLength={500}
              />
              {frontError && (
                <p className="text-sm text-red-500">{frontError}</p>
              )}
              <div className="text-right text-sm text-muted-foreground">
                {front.length}/500 characters
              </div>
            </div>

            <div className="space-y-2">
              <label htmlFor="back" className="text-sm font-medium">
                Back (Answer)
              </label>
              <Textarea
                id="back"
                value={back}
                onChange={handleBackChange}
                placeholder="Enter the answer or definition (max 200 characters)"
                className="resize-y"
                maxLength={200}
              />
              {backError && <p className="text-sm text-red-500">{backError}</p>}
              <div className="text-right text-sm text-muted-foreground">
                {back.length}/200 characters
              </div>
            </div>
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={onCancel}>
              Cancel
            </Button>
            <Button type="submit">Save Changes</Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
