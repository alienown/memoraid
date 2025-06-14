import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { toast } from "sonner";
import { FlashcardsList, FlashcardsListSkeleton } from "./FlashcardsList";
import { CreateFlashcardModal } from "./CreateFlashcardModal";
import { EditFlashcardModal } from "./EditFlashcardModal";
import { ConfirmationDialog } from "@/components/ConfirmationDialog";
import { Pagination } from "./Pagination";
import { EditFlashcardData } from "./types";
import { apiClient } from "@/api/apiClient";

export function Flashcards() {
  const [flashcards, setFlashcards] = useState<{
    items: Array<{ id: number; front: string; back: string }>;
    total: number;
  }>({
    items: [],
    total: 0,
  });
  const [isLoading, setIsLoading] = useState(true);

  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isDeleteConfirmationOpen, setIsDeleteConfirmationOpen] =
    useState(false);
  const [isDeleteProcessing, setIsDeleteProcessing] = useState(false);
  const [selectedFlashcard, setSelectedFlashcard] =
    useState<EditFlashcardData | null>(null);
  const [flashcardIdToDelete, setFlashcardIdToDelete] = useState<number | null>(
    null
  );

  const fetchFlashcards = async () => {
    setIsLoading(true);

    try {
      const response = await apiClient.flashcards.getFlashcards({
        PageNumber: currentPage,
        PageSize: pageSize,
      });

      if (response.data.isSuccess && response.data.data) {
        setFlashcards(response.data.data);
      } else {
        toast.error(
          "Failed to load flashcards. Please refresh the page to try again."
        );
      }
    } catch {
      toast.error(
        "Failed to load flashcards. Please refresh the page to try again."
      );
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchFlashcards();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentPage, pageSize]);

  const handleFlashcardCreated = async () => {
    fetchFlashcards();
  };

  const handleEditFlashcard = (id: number) => {
    const flashcard = flashcards.items.find((card) => card.id === id);
    if (flashcard) {
      setSelectedFlashcard({
        id: flashcard.id,
        front: flashcard.front,
        back: flashcard.back,
      });
      setIsEditModalOpen(true);
    }
  };

  const handleFlashcardEdited = async () => {
    fetchFlashcards();
  };

  const handleDeleteFlashcard = (id: number) => {
    setFlashcardIdToDelete(id);
    setIsDeleteConfirmationOpen(true);
  };

  const confirmDeleteFlashcard = async () => {
    if (flashcardIdToDelete === null) return;

    setIsDeleteProcessing(true);

    try {
      const response = await apiClient.flashcards.deleteFlashcard(
        flashcardIdToDelete
      );

      if (response.data.isSuccess) {
        toast.success("Flashcard deleted successfully");
        setIsDeleteConfirmationOpen(false);
        fetchFlashcards();
      } else if (response.data.errors.length > 0) {
        response.data.errors.forEach((error) => {
          toast.error(error.message);
        });
      } else {
        toast.error("Failed to delete flashcard");
      }
    } catch {
      toast.error("Failed to delete flashcard");
    } finally {
      setIsDeleteProcessing(false);
      setFlashcardIdToDelete(null);
    }
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePageSizeChange = (size: number) => {
    setPageSize(size);
    setCurrentPage(1);
  };

  return (
    <div className="container mx-auto py-8 max-w-6xl">
      <div className="flex justify-between items-center mb-5">
        <Button onClick={() => setIsCreateModalOpen(true)} disabled={isLoading}>
          <Plus className="h-4 w-4" />
          Create flashcard
        </Button>
      </div>

      <div className="relative">
        {isLoading && (
          <FlashcardsListSkeleton
            count={flashcards.items.length ? flashcards.items.length : 6}
          />
        )}
        {!isLoading && flashcards.items.length === 0 ? (
          <div className="text-center py-10 border rounded-md">
            <p className="text-gray-500">
              No flashcards found. Create your first one!
            </p>
          </div>
        ) : (
          !isLoading && (
            <FlashcardsList
              flashcards={flashcards.items}
              onEdit={handleEditFlashcard}
              onDelete={handleDeleteFlashcard}
            />
          )
        )}
      </div>

      {flashcards.total > 0 && (
        <div className="mt-5">
          <Pagination
            currentPage={currentPage}
            totalItems={flashcards.total}
            pageSize={pageSize}
            onPageChange={handlePageChange}
            onPageSizeChange={handlePageSizeChange}
          />
        </div>
      )}

      <CreateFlashcardModal
        isOpen={isCreateModalOpen}
        onCreated={handleFlashcardCreated}
        onCancel={() => setIsCreateModalOpen(false)}
      />

      {selectedFlashcard && (
        <EditFlashcardModal
          isOpen={isEditModalOpen}
          flashcard={selectedFlashcard}
          onEdited={handleFlashcardEdited}
          onCancel={() => setIsEditModalOpen(false)}
        />
      )}

      <ConfirmationDialog
        isOpen={isDeleteConfirmationOpen}
        message="Are you sure you want to delete this flashcard? This action cannot be undone."
        onConfirm={confirmDeleteFlashcard}
        onCancel={() => setIsDeleteConfirmationOpen(false)}
        isLoading={isDeleteProcessing}
      />
    </div>
  );
}
