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
import { EditFlashcardData } from "./types";
import { apiClient } from "@/api/apiClient";
import { toast } from "sonner";
import { validateFlashcard } from "@/core/validation/flashcards";
import { Loader2 } from "lucide-react";

export interface EditFlashcardModalProps {
  isOpen: boolean;
  flashcard: EditFlashcardData;
  onEdited: () => void;
  onCancel: () => void;
}

export function EditFlashcardModal({
  isOpen,
  flashcard,
  onEdited,
  onCancel,
}: EditFlashcardModalProps) {
  const [front, setFront] = useState("");
  const [back, setBack] = useState("");
  const [frontError, setFrontError] = useState<string | null>(null);
  const [backError, setBackError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (flashcard) {
      setFront(flashcard.front);
      setBack(flashcard.back);
      setFrontError(null);
      setBackError(null);
    }
  }, [flashcard]);

  const validateForm = (): boolean => {
    const { isValid, frontError, backError } = validateFlashcard(front, back);
    setFrontError(frontError);
    setBackError(backError);
    return isValid;
  };

  const editFlashcard = async () => {
    setIsLoading(true);

    try {
      const response = await apiClient.flashcards.updateFlashcard(
        flashcard.id,
        { front, back }
      );

      if (response.data.isSuccess) {
        toast.success("Flashcard edited successfully");
        handleDialogClose();
        onEdited();
      } else if (response.data.errors.length > 0) {
        response.data.errors.forEach((error) => {
          toast.error(error.message);
        });
      } else {
        toast.error("Failed to edit flashcard");
      }
    } catch {
      toast.error("Failed to edit flashcard");
    } finally {
      setIsLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (validateForm()) {
      await editFlashcard();
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

  const handleDialogClose = () => {
    onCancel();
  };

  return (
    <Dialog open={isOpen} onOpenChange={(open) => !open && handleDialogClose()}>
      <DialogContent className="sm:max-w-[900px]">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Edit Flashcard</DialogTitle>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="space-y-2">
              <label htmlFor="front" className="font-medium">
                Front (Question)
              </label>
              <Textarea
                id="front"
                value={front}
                onChange={handleFrontChange}
                placeholder="Enter the question or term (max 500 characters)"
                className="resize-y h-30"
                maxLength={500}
                disabled={isLoading}
              />
              {frontError && (
                <p className="text-sm text-red-500">{frontError}</p>
              )}
              <div className="text-right text-sm text-muted-foreground">
                {front.length}/500 characters
              </div>
            </div>

            <div className="space-y-2">
              <label htmlFor="back" className="font-medium">
                Back (Answer)
              </label>
              <Textarea
                id="back"
                value={back}
                onChange={handleBackChange}
                placeholder="Enter the answer or definition (max 200 characters)"
                className="resize-y"
                maxLength={200}
                disabled={isLoading}
              />
              {backError && <p className="text-sm text-red-500">{backError}</p>}
              <div className="text-right text-sm text-muted-foreground">
                {back.length}/200 characters
              </div>
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={handleDialogClose}
              disabled={isLoading}
            >
              Cancel
            </Button>
            <Button type="submit" disabled={isLoading}>
              {isLoading ? (
                <>
                  <Loader2 className="animate-spin" />
                  Saving...
                </>
              ) : (
                "Save Changes"
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
