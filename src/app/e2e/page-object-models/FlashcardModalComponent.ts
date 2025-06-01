import { Page, Locator } from "@playwright/test";

export class FlashcardModalComponent {
  readonly page: Page;
  readonly modal: Locator;
  readonly frontTextarea: Locator;
  readonly backTextarea: Locator;
  readonly saveButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.modal = page.getByRole("dialog", {
      name: /edit flashcard/i,
    });
    this.frontTextarea = this.modal.getByLabel("Front (Question)");
    this.backTextarea = this.modal.getByLabel("Back (Answer)");
    this.saveButton = this.modal.getByRole("button", {
      name: /save changes/i,
    });
  }

  async isOpen() {
    return this.modal.isVisible();
  }

  async fillForm(front: string, back: string) {
    await this.frontTextarea.fill(front);
    await this.backTextarea.fill(back);
  }

  async save() {
    await this.saveButton.click();
  }
}
