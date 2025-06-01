import { Page, Locator } from "@playwright/test";

export class NavbarComponent {
  readonly page: Page;
  readonly generateLink: Locator;
  readonly flashcardsLink: Locator;

  constructor(page: Page) {
    this.page = page;
    this.generateLink = page.getByRole("link", { name: "Generate" });
    this.flashcardsLink = page.getByRole("link", { name: "My Flashcards" });
  }

  async goToGeneratePage() {
    await this.generateLink.click();
  }

  async goToFlashcardsPage() {
    await this.flashcardsLink.click();
  }
}
