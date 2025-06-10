import { Api } from "./api";

export const apiClient = new Api({
  baseURL: process.env.API_URL,
});
