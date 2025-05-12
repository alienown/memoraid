import { InternalAxiosRequestConfig, AxiosResponse } from "axios";
import { tokenStorage } from "./tokenStorage";
import { apiClient } from "@/api/apiClient";

let unauthenticatedHandler: (() => void) | null = null;

export const setupUnauthenticatedHandler = (handler: () => void) => {
  unauthenticatedHandler = handler;
};

export const setupAxiosInterceptors = () => {
  apiClient.instance.interceptors.request.use(addAuthTokenToRequest);
  apiClient.instance.interceptors.response.use(
    handleResponseError,
    (response) => Promise.reject(response)
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

const handleResponseError = (response: AxiosResponse) => {
  if (response.status === 401 && unauthenticatedHandler) {
    unauthenticatedHandler();
  }

  return response;
};
