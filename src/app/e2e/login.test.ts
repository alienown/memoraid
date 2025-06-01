import { test } from "@playwright/test";
import { LoginPage } from "./page-object-models/LoginPage";

test.describe("Login functionality", () => {
  test("should be redirected to login page when accessing application as unauthenticated user", async ({
    page,
  }) => {
    // Arrange
    const appUrl = process.env.APP_URL!;

    // Act
    await page.goto(appUrl);

    // Assert
    await page.waitForURL("**/login");
  });

  test("should redirect to flashcard generation page when logging in with valid credentials", async ({
    page,
  }) => {
    // Arrange
    const loginPage = new LoginPage(page);

    const validEmail = process.env.TEST_USER_EMAIL!;
    const validPassword = process.env.TEST_USER_PASSWORD!;

    await loginPage.navigate();

    // Act
    await loginPage.login(validEmail, validPassword);

    // Assert
    await loginPage.waitForLoginSuccess();
    await page.waitForURL("**/generate");
  });
});
