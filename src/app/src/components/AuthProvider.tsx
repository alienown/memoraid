import { ReactNode, useEffect, useState } from "react";
import {
  login,
  logout,
  onAuthStateChanged,
  register,
  getToken,
} from "../core/auth/authService";
import { AuthContext } from "../core/auth/AuthContext";
import { AxiosResponse, InternalAxiosRequestConfig } from "axios";
import { apiClient } from "@/api/apiClient";

const addAuthTokenToRequest = async (config: InternalAxiosRequestConfig) => {
  const token = await getToken();

  if (token) {
    config.headers = config.headers || {};
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
};

const handleResponseError = async (response: AxiosResponse) => {
  if (response.status === 401) {
    await logout();
  }

  return response;
};

const setupAxiosAuthInterceptors = () => {
  apiClient.instance.interceptors.request.use(addAuthTokenToRequest);
  apiClient.instance.interceptors.response.use(
    handleResponseError,
    (response) => Promise.reject(response)
  );
};

export function AuthProvider({ children }: { children: ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const unsubscribe = onAuthStateChanged((isAuthenticated) => {
      setIsAuthenticated(isAuthenticated);
    });

    setupAxiosAuthInterceptors();

    return () => unsubscribe();
  }, []);

  const value = {
    isAuthenticated,
    register,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
