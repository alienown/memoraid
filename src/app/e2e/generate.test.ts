import { test, expect } from "@playwright/test";
import { GeneratePage } from "./page-object-models/GeneratePage";
import { FlashcardsPage } from "./page-object-models/FlashcardsPage";
import { FlashcardModalComponent } from "./page-object-models/FlashcardModalComponent";

test.describe("AI-Generated Flashcards", () => {
  test("should be redirected to flashcard generation page when accessing application as authenticated user", async ({
    page,
  }) => {
    // Arrange
    const generatePage = new GeneratePage(page);
    const appUrl = process.env.APP_URL!;

    // Act
    await page.goto(appUrl);

    // Assert
    await generatePage.header.waitFor({ state: "visible" });
  });

  test("should generate flashcards from text, accept, edit, and submit accepted flashcards, and then show them on my flashcards page", async ({
    page,
  }) => {
    // Arrange
    const generatePage = new GeneratePage(page);
    const flashcardsPage = new FlashcardsPage(page);
    const flashcardModal = new FlashcardModalComponent(page);
    const sampleText = `
      Spaced repetition is a learning technique that involves increasing intervals of time between reviews of previously learned material. 
      It exploits the psychological spacing effect, which demonstrates that learning is more effective when study sessions are spaced out over time.
      The technique is particularly effective for retaining information in long-term memory, as opposed to cramming which only benefits short-term recall.
      Hermann Ebbinghaus, a German psychologist, first identified the spacing effect in the 1880s through his experiments on memory.
    `;

    // Act
    await generatePage.navigate();
    await generatePage.generateFlashcards(sampleText);
    await generatePage.waitUntilFlashcardsGenerated();

    const firstCardFront = await generatePage.getFlashcardFrontText(0);
    const firstCardBack = await generatePage.getFlashcardBackText(0);

    const secondCardFront = await generatePage.getFlashcardFrontText(1);
    const secondCardBack = await generatePage.getFlashcardBackText(1);

    await generatePage.editFlashcard(0);
    await flashcardModal.waitForModelToBeOpened();

    const editedFront = `${firstCardFront} (Edited)`;
    const editedBack = `${firstCardBack} (Edited)`;
    await flashcardModal.fillForm(editedFront, editedBack);
    await flashcardModal.save();

    await generatePage.acceptFlashcard(0);
    await generatePage.acceptFlashcard(1);

    await generatePage.submitAcceptedFlashcards();
    await generatePage.waitUntilFlashcardsSubmitted();
    await flashcardsPage.navigate();
    await flashcardsPage.waitUntilFlashcardsLoaded();

    // Assert
    expect(await flashcardsPage.getFlashcardsCount()).toBe(2);

    // Verify first card content (edited)
    expect(await flashcardsPage.getFlashcardFrontText(0)).toBe(editedFront);
    expect(await flashcardsPage.getFlashcardBackText(0)).toBe(editedBack);

    // Verify second card content
    expect(await flashcardsPage.getFlashcardFrontText(1)).toBe(secondCardFront);
    expect(await flashcardsPage.getFlashcardBackText(1)).toBe(secondCardBack);
  });
});
