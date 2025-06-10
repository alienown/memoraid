import { Api } from "./api";

export const apiClient = new Api({
  baseURL: import.meta.env.VITE_API_URL,
  validateStatus: (status) => {
    return status >= 200 && status < 500;
  },
});
