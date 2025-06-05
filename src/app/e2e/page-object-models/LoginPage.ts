import { Page, Locator } from "@playwright/test";
import { BasePage } from "./BasePage";

export class LoginPage extends BasePage {
  readonly cardTitle: Locator;
  readonly emailInput: Locator;
  readonly passwordInput: Locator;
  readonly loginButton: Locator;
  readonly toastLoginSuccess: Locator;

  constructor(page: Page) {
    super(page, "/login");

    this.cardTitle = page.getByText(/login to memoraid/i);
    this.emailInput = page.getByLabel("Email");
    this.passwordInput = page.getByLabel("Password");
    this.loginButton = page.getByRole("button", { name: "Login" });
    this.toastLoginSuccess = page.getByText(/login successful!/i);
  }

  async login(email: string, password: string) {
    await this.emailInput.fill(email);
    await this.passwordInput.fill(password);
    await this.loginButton.click();
  }

  async waitForLoginSuccess() {
    await this.toastLoginSuccess.waitFor({
      state: "visible",
    });
  }
}
