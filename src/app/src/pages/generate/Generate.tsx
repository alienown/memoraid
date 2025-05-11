import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { toast } from "sonner";
import {
  GeneratedFlashcard,
  FlashcardSource,
  CreateFlashcardData,
  CreateFlashcardsRequest,
  GenerateFlashcardsRequest,
} from "@/api/api";
import { FlashcardList } from "./FlashcardList";
import { EditFlashcardModal } from "./EditFlashcardModal";
import { FlashcardData } from "./types";
import { apiClient } from "@/api/apiClient";

interface ModalState {
  isOpen: boolean;
  flashcardToEdit: FlashcardData | null;
}

const Generate = () => {
  // Local state variables
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
  const [error, setError] = useState<string | null>(null);

  // Function to handle text input changes
  const handleTextChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setSourceText(event.target.value);
    // Clear any previous error
    if (error) setError(null);
  };

  // Function to generate flashcards via API
  const handleGenerateFlashcards = async () => {
    // Validate input
    if (!sourceText.trim()) {
      setError("Please enter text to generate flashcards.");
      return;
    }

    if (sourceText.length > 10000) {
      setError("Text is too long. Maximum 10,000 characters allowed.");
      return;
    }

    setIsGenerating(true);
    setError(null);

    try {
      // Prepare request payload
      const request: GenerateFlashcardsRequest = {
        sourceText: sourceText.trim(),
      };

      // Call the API
      const response = await apiClient.flashcards.generateFlashcards(request);

      // Check if the API call was successful
      if (response.data.isSuccess && response.data.data) {
        // Map the API response to our component's flashcards state
        const flashcards = response.data.data.flashcards.map(
          (card: GeneratedFlashcard) => ({
            front: card.front,
            back: card.back,
            source: FlashcardSource.AIFull,
            generationId: response.data.data!.generationId,
            isAccepted: false,
          })
        );

        setGeneratedFlashcards(flashcards);

        // Show success toast
        if (flashcards.length > 0) {
          toast.success(
            `Generated ${flashcards.length} flashcards successfully!`
          );
        } else {
          toast.info(
            "No flashcards could be generated from the provided text."
          );
        }
      } else {
        // Handle API error
        const errorMessage =
          response.data.errors && response.data.errors.length > 0
            ? response.data.errors[0].message
            : "Failed to generate flashcards. Please try again.";

        setError(errorMessage);
        toast.error(errorMessage);
      }
    } catch (err) {
      console.error("Error generating flashcards:", err);
      setError(
        "An error occurred while generating flashcards. Please try again."
      );
      toast.error("Failed to connect to the server. Please try again later.");
    } finally {
      setIsGenerating(false);
    }
  };

  // Function to handle accepting a flashcard
  const handleAcceptFlashcard = (index: number) => {
    setGeneratedFlashcards((currentCards) =>
      currentCards.map((card, i) =>
        i === index ? { ...card, isAccepted: true } : card
      )
    );
  };

  // Function to handle rejecting a flashcard
  const handleRejectFlashcard = (index: number) => {
    setGeneratedFlashcards((currentCards) =>
      currentCards.map((card, i) =>
        i === index ? { ...card, isAccepted: false } : card
      )
    );
  };

  // Function to open edit modal for a flashcard
  const handleEditFlashcard = (index: number) => {
    setModalState({
      isOpen: true,
      flashcardToEdit: generatedFlashcards[index],
    });
  };

  // Function to close the edit modal
  const handleCloseModal = () => {
    setModalState({
      isOpen: false,
      flashcardToEdit: null,
    });
  };

  // Function to save edited flashcard
  const handleSaveEdit = (editedCard: { front: string; back: string }) => {
    // Update the flashcard in the state
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

    // Close the modal
    handleCloseModal();

    // Show success toast
    toast.success("Flashcard updated successfully!");
  };

  // Function to submit accepted flashcards
  const handleSubmitFlashcards = async () => {
    const acceptedCards = generatedFlashcards.filter((card) => card.isAccepted);

    if (acceptedCards.length === 0) {
      setError("Please accept at least one flashcard before submitting.");
      return;
    }

    setIsSubmitting(true);
    setError(null);

    try {
      // Map to API request format
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

      // Call the API
      const response = await apiClient.flashcards.createFlashcards(request);

      // Handle response
      if (response.data.isSuccess) {
        // Show success message
        toast.success(`${acceptedCards.length} flashcards saved successfully!`);

        // Reset state
        setSourceText("");
        setGeneratedFlashcards([]);
      } else {
        // Handle API error
        const errorMessage =
          response.data.errors && response.data.errors.length > 0
            ? response.data.errors[0].message
            : "Failed to save flashcards. Please try again.";

        setError(errorMessage);
        toast.error(errorMessage);
      }
    } catch (err) {
      console.error("Error submitting flashcards:", err);
      setError("An error occurred while saving flashcards. Please try again.");
      toast.error("Failed to connect to the server. Please try again later.");
    } finally {
      setIsSubmitting(false);
    }
  };

  const acceptedCount = generatedFlashcards.filter(
    (card) => card.isAccepted
  ).length;

  return (
    <div className="container mx-auto p-4 max-w-7xl">
      <h1 className="text-3xl font-bold mb-6">Generate Flashcards</h1>

      <div className="mb-6 space-y-2">
        <label htmlFor="source-text" className="text-sm font-medium">
          Enter Text
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
        {error && <p className="text-sm text-red-500">{error}</p>}
        <div className="text-right text-sm text-muted-foreground">
          {sourceText.length}/10,000 characters
        </div>
      </div>

      <Button
        onClick={handleGenerateFlashcards}
        disabled={isGenerating || !sourceText.trim()}
        className="mb-6"
      >
        {isGenerating ? "Generating..." : "Generate Flashcards"}
      </Button>

      {generatedFlashcards.length > 0 && (
        <div className="mb-6 w-full">
          <FlashcardList
            flashcards={generatedFlashcards}
            onAccept={handleAcceptFlashcard}
            onReject={handleRejectFlashcard}
            onEdit={handleEditFlashcard}
          />

          <div className="mt-6">
            <Button
              onClick={handleSubmitFlashcards}
              disabled={isSubmitting || acceptedCount === 0}
            >
              {isSubmitting
                ? "Submitting..."
                : `Submit ${acceptedCount} Accepted Flashcard${
                    acceptedCount !== 1 ? "s" : ""
                  }`}
            </Button>
          </div>
        </div>
      )}

      {/* Edit flashcard modal */}
      <EditFlashcardModal
        isOpen={modalState.isOpen}
        flashcard={modalState.flashcardToEdit}
        onSave={handleSaveEdit}
        onCancel={handleCloseModal}
      />
    </div>
  );
};

export default Generate;
