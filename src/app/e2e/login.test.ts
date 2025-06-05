import { test } from "@playwright/test";
import { LoginPage } from "./page-object-models";

test.describe("Login", () => {
  test.use({ storageState: { cookies: [], origins: [] } });
  
  test("should be redirected to login page when accessing application as unauthenticated user", async ({
    page,
  }) => {
    // Arrange
    const loginPage = new LoginPage(page);
    const appUrl = process.env.APP_URL!;

    // Act
    await page.goto(appUrl);

    // Assert
    await loginPage.cardTitle.waitFor({ state: "visible" });
  });
});