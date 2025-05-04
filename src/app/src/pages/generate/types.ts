import { FlashcardSource } from "@/api/api";

export interface FlashcardData {
  front: string;
  back: string;
  source: FlashcardSource;
  generationId?: number;
  isAccepted: boolean;
}