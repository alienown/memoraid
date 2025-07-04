import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { toast } from "sonner";
import { Loader2 } from "lucide-react";
import {
  GeneratedFlashcard,
  FlashcardSource,
  CreateFlashcardData,
  CreateFlashcardsRequest,
  GenerateFlashcardsRequest,
} from "@/api/api";
import { FlashcardList, FlashcardsListSkeleton } from "./FlashcardList";
import { EditFlashcardModal } from "./EditFlashcardModal";
import { FlashcardData } from "./types";
import { apiClient } from "@/api/apiClient";

interface ModalState {
  isOpen: boolean;
  flashcardToEdit: FlashcardData | null;
}

const Generate = () => {
  const [sourceText, setSourceText] = useState<string>("");
  const [generatedFlashcards, setGeneratedFlashcards] = useState<
    FlashcardData[]
  >([]);
  const [modalState, setModalState] = useState<ModalState>({
    isOpen: false,
    flashcardToEdit: null,
  });
  const [isGenerating, setIsGenerating] = useState<boolean>(false);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [sourceTextError, setSourceTextError] = useState<string | null>(null);

  const handleTextChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setSourceText(event.target.value);

    if (sourceTextError) setSourceTextError(null);
  };

  const handleGenerateFlashcards = async () => {
    if (!sourceText.trim()) {
      setSourceTextError("Please enter text to generate flashcards.");
      return;
    }

    if (sourceText.length > 10000) {
      setSourceTextError(
        "Text is too long. Maximum 10,000 characters allowed."
      );
      return;
    }

    setIsGenerating(true);
    setSourceTextError(null);

    try {
      const request: GenerateFlashcardsRequest = {
        sourceText: sourceText.trim(),
      };

      const response = await apiClient.flashcards.generateFlashcards(request);

      if (response.data.isSuccess) {
        if (
          !response.data.data ||
          response.data.data?.flashcards.length === 0
        ) {
          toast.error("No flashcards generated. Please try again.");
        } else {
          const flashcards = response.data.data.flashcards.map(
            (card: GeneratedFlashcard) => ({
              front: card.front,
              back: card.back,
              source: FlashcardSource.AIFull,
              generationId: response.data.data!.generationId,
              isAccepted: false,
              errors: [],
            })
          );

          setGeneratedFlashcards(flashcards);

          toast.success("Flashcards generated successfully!");
        }
      } else if (response.data.errors.length > 0) {
        response.data.errors?.forEach((error) => {
          toast.error(error.message);
        });
      } else {
        toast.error("Failed to generate flashcards. Please try again.");
      }
    } catch {
      toast.error("Failed to generate flashcards. Please try again.");
    } finally {
      setIsGenerating(false);
    }
  };

  const handleAcceptFlashcard = (index: number) => {
    setGeneratedFlashcards((currentCards) =>
      currentCards.map((card, i) =>
        i === index ? { ...card, isAccepted: true } : card
      )
    );
  };

  const handleRejectFlashcard = (index: number) => {
    setGeneratedFlashcards((currentCards) =>
      currentCards.map((card, i) =>
        i === index ? { ...card, isAccepted: false } : card
      )
    );
  };

  const handleEditFlashcard = (index: number) => {
    setModalState({
      isOpen: true,
      flashcardToEdit: generatedFlashcards[index],
    });
  };

  const handleCloseModal = () => {
    setModalState({
      isOpen: false,
      flashcardToEdit: null,
    });
  };

  const handleSaveEdit = (editedCard: { front: string; back: string }) => {
    setGeneratedFlashcards((currentCards) =>
      currentCards.map((card) =>
        card === modalState.flashcardToEdit
          ? {
              ...card,
              front: editedCard.front,
              back: editedCard.back,
              source: FlashcardSource.AIEdited,
            }
          : card
      )
    );

    handleCloseModal();

    toast.success("Flashcard edited successfully");
  };

  const handleSubmitFlashcards = async () => {
    const acceptedCards = generatedFlashcards.filter((card) => card.isAccepted);

    setIsSubmitting(true);
    setSourceTextError(null);

    try {
      const flashcardsToCreate: CreateFlashcardData[] = acceptedCards.map(
        (card) => ({
          front: card.front,
          back: card.back,
          source: card.source,
          generationId: card.generationId,
        })
      );

      const request: CreateFlashcardsRequest = {
        flashcards: flashcardsToCreate,
      };

      const response = await apiClient.flashcards.createFlashcards(request);

      if (response.data.isSuccess) {
        toast.success("Flashcards saved successfully!");

        setSourceText("");
        setGeneratedFlashcards([]);
      } else if (response.data.errors.length > 0) {
        response.data.errors.forEach((error) => {
          toast.error(error.message);
        });
      } else {
        toast.error("Failed to save flashcards. Please try again.");
      }
    } catch {
      toast.error("Failed to save flashcards. Please try again.");
    } finally {
      setIsSubmitting(false);
    }
  };

  const acceptedCount = generatedFlashcards.filter(
    (card) => card.isAccepted
  ).length;

  return (
    <div className="container mx-auto p-4 max-w-7xl">
      <div className="mb-6 space-y-2">
        <label htmlFor="source-text" className="font-medium">
          Source text
        </label>
        <Textarea
          id="source-text"
          value={sourceText}
          onChange={handleTextChange}
          placeholder="Paste your text here (up to 10,000 characters)"
          maxLength={10000}
          disabled={isGenerating}
          className="min-h-[200px] resize-y"
        />
        {sourceTextError && (
          <p className="text-sm text-red-500">{sourceTextError}</p>
        )}
        <div className="text-right text-sm text-muted-foreground">
          {sourceText.length}/10,000 characters
        </div>
      </div>
      <Button
        onClick={handleGenerateFlashcards}
        disabled={isGenerating || isSubmitting || !sourceText.trim()}
        className="mb-6"
      >
        {isGenerating ? (
          <>
            <Loader2 className="animate-spin" />
            <span className="mr-2">Generating...</span>
          </>
        ) : (
          "Generate Flashcards"
        )}
      </Button>
      <div className="mb-6 w-full relative">
        {isGenerating && (
          <FlashcardsListSkeleton
            count={generatedFlashcards.length ? generatedFlashcards.length : 6}
          />
        )}
        <FlashcardList
          flashcards={generatedFlashcards}
          disabled={isSubmitting}
          onAccept={handleAcceptFlashcard}
          onReject={handleRejectFlashcard}
          onEdit={handleEditFlashcard}
        />
      </div>
      {generatedFlashcards.length > 0 && (
        <div className="mt-6">
          <Button
            onClick={handleSubmitFlashcards}
            disabled={isGenerating || isSubmitting || acceptedCount === 0}
          >
            {isSubmitting ? (
              <>
                <Loader2 className="animate-spin" />
                Submitting...
              </>
            ) : (
              "Submit accepted flashcards"
            )}
          </Button>
        </div>
      )}
      <EditFlashcardModal
        isOpen={modalState.isOpen}
        flashcard={modalState.flashcardToEdit}
        onEdited={handleSaveEdit}
        onCancel={handleCloseModal}
      />
    </div>
  );
};

export default Generate;
