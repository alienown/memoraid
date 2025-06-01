import { Page, Locator } from "@playwright/test";
import { BasePage } from "./BasePage";

export class FlashcardsPage extends BasePage {
  readonly flashcardItems: Locator;
  readonly paginationContainer: Locator;
  readonly loadingSpinner: Locator;

  constructor(page: Page) {
    super(page, "/flashcards");
    this.flashcardItems = page.locator('[data-testid="flashcard-item"]');
    this.paginationContainer = page.locator('[data-testid="pagination"]');
    this.loadingSpinner = page.getByTestId("loading-spinner");
  }

  async waitUntilFlashcardsLoaded() {
    await this.loadingSpinner.waitFor({ state: "detached" });
  }

  async toggleFlashcardBackVisibility(index: number) {
    const flashcard = this.flashcardItems.nth(index);
    const viewButton = flashcard.getByTitle(/show answer|hide answer/i);
    await viewButton.click();
  }

  async isAnswerVisible(index: number): Promise<boolean> {
    const flashcard = this.flashcardItems.nth(index);
    const viewButton = flashcard.getByTitle(/hide answer/i);
    return await viewButton.isVisible();
  }

  async getFlashcardFrontText(index: number) {
    const flashcard = this.flashcardItems.nth(index);

    if (await this.isAnswerVisible(index)) {
      await this.toggleFlashcardBackVisibility(index);
    }

    return flashcard.getByTestId("flashcard-front-text").innerText();
  }

  async getFlashcardBackText(index: number) {
    const flashcard = this.flashcardItems.nth(index);

    if (!(await this.isAnswerVisible(index))) {
      await this.toggleFlashcardBackVisibility(index);
    }

    return flashcard.getByTestId("flashcard-back-text").innerText();
  }
}
