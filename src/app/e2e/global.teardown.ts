import { test as teardown } from "@playwright/test";
import { apiClient } from "@/api/apiClient";
import { getToken, login } from "./authService";

teardown("cleanup e2e user data", async () => {
  try {
    await login();

    const token = await getToken();

    await apiClient.users.deleteUser({
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  } catch (error) {
    console.error("Failed to delete user data:", error);
  }
});
