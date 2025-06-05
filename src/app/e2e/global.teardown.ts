import { test as teardown } from "@playwright/test";
import { getToken, login } from "./authService";
import { Api } from "./api";

teardown("cleanup e2e user data", async () => {
  try {
    const api = new Api();

    await login();

    const token = await getToken();

    await api.users.deleteUser({
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  } catch (error) {
    console.error("Failed to delete user data:", error);
  }
});
