export interface FlashcardValidationResult {
  isValid: boolean;
  frontError: string | null;
  backError: string | null;
}

export function validateFlashcard(
  front: string,
  back: string
): FlashcardValidationResult {
  let isValid = true;
  let frontError: string | null = null;
  let backError: string | null = null;

  if (!front.trim()) {
    frontError = "Front text is required";
    isValid = false;
  } else if (front.length > 500) {
    frontError = "Front text cannot exceed 500 characters";
    isValid = false;
  }

  if (!back.trim()) {
    backError = "Back text is required";
    isValid = false;
  } else if (back.length > 200) {
    backError = "Back text cannot exceed 200 characters";
    isValid = false;
  }

  return {
    isValid,
    frontError,
    backError,
  };
}
