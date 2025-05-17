import { InternalAxiosRequestConfig, AxiosResponse } from "axios";
import { tokenStorage } from "./tokenStorage";
import { apiClient } from "@/api/apiClient";

export const setupAxiosAuthInterceptors = (unauthenticatedHandler: () => Promise<void>) => {
  apiClient.instance.interceptors.request.use(addAuthTokenToRequest);
  apiClient.instance.interceptors.response.use(
    (response) => handleResponseError(response, unauthenticatedHandler),
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

const handleResponseError = async (
  response: AxiosResponse,
  unauthenticatedHandler: () => Promise<void>
) => {
  if (response.status === 401) {
    await unauthenticatedHandler();
  }

  return response;
};
