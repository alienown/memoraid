import { Page, Locator } from "@playwright/test";
import { BasePage } from "./BasePage";

export class GeneratePage extends BasePage {
  readonly sourceTextArea: Locator;
  readonly generateButton: Locator;
  readonly flashcardItems: Locator;
  readonly toastGenerateSuccess: Locator;
  readonly submitAcceptedButton: Locator;
  readonly toastSubmitSuccess: Locator;

  constructor(page: Page) {
    super(page, "/generate");
    this.sourceTextArea = page.getByRole("textbox", { name: /enter text/i });
    this.generateButton = page.getByRole("button", {
      name: /generate flashcards/i,
    });
    this.flashcardItems = page.getByTestId("flashcard-item");
    this.toastGenerateSuccess = page.getByText(
      /flashcards generated successfully/i
    );
    this.toastSubmitSuccess = page.getByText(
      /flashcards saved successfully/i
    );
    this.submitAcceptedButton = page.getByRole("button", {
      name: /submit accepted flashcards/i,
    });
  }

  async generateFlashcards(text: string) {
    await this.sourceTextArea.fill(text);
    await this.generateButton.click();
  }

  async waitUntilFlashcardsGenerated() {
    await this.toastGenerateSuccess.waitFor({
      state: "visible",
    });
  }

  async submitAcceptedFlashcards() {
    await this.submitAcceptedButton.click();
  }

  async waitUntilFlashcardsSubmitted() {
    await this.toastSubmitSuccess.waitFor({
      state: "visible",
    });
  }

  async acceptFlashcard(index: number) {
    const flashcard = this.flashcardItems.nth(index);
    await flashcard.getByTitle(/accept/i).click();
  }

  async rejectFlashcard(index: number) {
    const flashcard = this.flashcardItems.nth(index);
    await flashcard.getByTitle(/reject/i).click();
  }

  async editFlashcard(index: number) {
    const flashcard = this.flashcardItems.nth(index);
    await flashcard.getByTitle(/edit/i).click();
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
