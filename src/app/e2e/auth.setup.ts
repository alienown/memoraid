import { test as setup } from "@playwright/test";
import { LoginPage } from "./page-object-models/LoginPage";

setup("authenticate", async ({ page }) => {
  const loginPage = new LoginPage(page);
  
  await loginPage.navigate();

  await loginPage.login(
    process.env.TEST_USER_EMAIL || "",
    process.env.TEST_USER_PASSWORD || ""
  );

  await loginPage.waitForLoginSuccess();
  
  await page.context().storageState({ path: "./e2e/test-results/auth.json", indexedDB: true });
});
