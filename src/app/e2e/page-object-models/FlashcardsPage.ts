import { Page, Locator } from "@playwright/test";
import { BasePage } from "./BasePage";

export class FlashcardsPage extends BasePage {
  readonly flashcardItems: Locator;
  readonly noFlashcardsText: Locator;

  constructor(page: Page) {
    super(page, "/flashcards");
    this.flashcardItems = page.getByTestId("flashcard-item");
    this.noFlashcardsText = page.getByText(
      /No flashcards found. Create your first one!/i
    );
  }

  async getFlashcardsCount(): Promise<number> {
    return await this.flashcardItems.count();
  }

  async waitUntilFlashcardsLoaded() {
    await Promise.race([
      this.flashcardItems
        .first()
        .waitFor({ state: "visible" })
        .catch(() => {}),
      this.noFlashcardsText.waitFor({ state: "visible" }).catch(() => {}),
    ]);
  }

  async flipFlashcard(index: number) {
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
      await this.flipFlashcard(index);
    }

    return await flashcard.getByTestId("flashcard-front-text").innerText();
  }

  async getFlashcardBackText(index: number) {
    const flashcard = this.flashcardItems.nth(index);

    if (!(await this.isAnswerVisible(index))) {
      await this.flipFlashcard(index);
    }

    return await flashcard.getByTestId("flashcard-back-text").innerText();
  }
}
