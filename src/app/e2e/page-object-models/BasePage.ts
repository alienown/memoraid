import { Page } from "@playwright/test";

export class BasePage {
  readonly page: Page;
  readonly pagePath: string;

  constructor(page: Page, pagePath: string) {
    this.page = page;
    this.pagePath = pagePath;
  }

  async navigate() {
    const baseUrl = process.env.APP_URL || '';
    await this.page.goto(`${baseUrl}${this.pagePath}`);
  }
}
