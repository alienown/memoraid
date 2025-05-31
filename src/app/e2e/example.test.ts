import { test, expect } from '@playwright/test';

test('should go to the login page when user is not logged in', async ({ page }) => {
  // Arrange
  const appUrl = process.env.APP_URL!;

  // Act
  await page.goto(appUrl);

  // Assert
  await expect(page.getByText(/Login to Memoraid/)).toBeVisible();
});