import { Api } from "./api";

export const apiClient = new Api({
  validateStatus: (status) => {
    return status >= 200 && status < 500;
  }
});
