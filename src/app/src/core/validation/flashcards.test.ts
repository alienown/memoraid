import { describe, it, expect } from "vitest";
import { validateFlashcard } from "./flashcards";

describe("validateFlashcard", () => {
  it("should return valid result when both front and back are valid", () => {
    // Arrange
    const front = "Valid front text";
    const back = "Valid back text";

    // Act
    const result = validateFlashcard(front, back);

    // Assert
    expect(result.isValid).toBe(true);
    expect(result.frontError).toBeNull();
    expect(result.backError).toBeNull();
  });

  describe("Front text validation", () => {
    it("should return invalid result when front text is empty", () => {
      // Arrange
      const front = "";
      const back = "Valid back text";

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(false);
      expect(result.frontError).toBe("Front text is required");
      expect(result.backError).toBeNull();
    });

    it("should return invalid result when front text is whitespace", () => {
      // Arrange
      const front = "   ";
      const back = "Valid back text";

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(false);
      expect(result.frontError).toBe("Front text is required");
      expect(result.backError).toBeNull();
    });

    it("should return invalid result when front text exceeds 500 characters", () => {
      // Arrange
      const front = "a".repeat(501);
      const back = "Valid back text";

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(false);
      expect(result.frontError).toBe("Front text cannot exceed 500 characters");
      expect(result.backError).toBeNull();
    });

    it("should return valid front result when front text is exactly 500 characters", () => {
      // Arrange
      const front = "a".repeat(500);
      const back = "Valid back text";

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(true);
      expect(result.frontError).toBeNull();
      expect(result.backError).toBeNull();
    });
  });

  describe("Back text validation", () => {
    it("should return invalid result when back text is empty", () => {
      // Arrange
      const front = "Valid front text";
      const back = "";

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(false);
      expect(result.frontError).toBeNull();
      expect(result.backError).toBe("Back text is required");
    });

    it("should return invalid result when back text is whitespace", () => {
      // Arrange
      const front = "Valid front text";
      const back = "   ";

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(false);
      expect(result.frontError).toBeNull();
      expect(result.backError).toBe("Back text is required");
    });

    it("should return invalid result when back text exceeds 200 characters", () => {
      // Arrange
      const front = "Valid front text";
      const back = "a".repeat(201);

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(false);
      expect(result.frontError).toBeNull();
      expect(result.backError).toBe("Back text cannot exceed 200 characters");
    });

    it("should return valid back result when back text is exactly 200 characters", () => {
      // Arrange
      const front = "Valid front text";
      const back = "a".repeat(200);

      // Act
      const result = validateFlashcard(front, back);

      // Assert
      expect(result.isValid).toBe(true);
      expect(result.frontError).toBeNull();
      expect(result.backError).toBeNull();
    });
  });

  it("should return multiple errors when both front and back are empty", () => {
    // Arrange
    const front = "";
    const back = "";

    // Act
    const result = validateFlashcard(front, back);

    // Assert
    expect(result.isValid).toBe(false);
    expect(result.frontError).toBe("Front text is required");
    expect(result.backError).toBe("Back text is required");
  });
});
