import { InternalAxiosRequestConfig, AxiosError } from "axios";
import { tokenStorage } from "./tokenStorage";
import { apiClient } from "@/api/apiClient";

let forbiddenHandler: (() => void) | null = null;

export const setupForbiddenHandler = (handler: () => void) => {
  forbiddenHandler = handler;
};

export const setupAxiosInterceptors = () => {
  apiClient.instance.interceptors.request.use(addAuthTokenToRequest);
  apiClient.instance.interceptors.response.use(
    (response) => response,
    handleResponseError
  );
};

const addAuthTokenToRequest = (config: InternalAxiosRequestConfig) => {
  const token = tokenStorage.getToken();

  if (token) {
    config.headers = config.headers || {};
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
};

const handleResponseError = (error: AxiosError) => {
  if (error.response?.status === 403 && forbiddenHandler) {
    forbiddenHandler();
  }

  return Promise.reject(error);
};
