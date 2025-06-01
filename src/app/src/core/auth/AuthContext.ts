import { createContext } from "react";
import { AuthResponse } from "./authService";

interface AuthContextType {
  isAuthenticated: boolean | null;
  register: (email: string, password: string) => Promise<AuthResponse>;
  login: (email: string, password: string) => Promise<AuthResponse>;
  logout: () => Promise<void>;
}

export function createDefaultAuthContext(): AuthContextType {
  return {
    isAuthenticated: null,
    register: async () => ({ isSuccess: false }),
    login: async () => ({ isSuccess: false }),
    logout: async () => {},
  };
}

export const AuthContext = createContext<AuthContextType>(createDefaultAuthContext());
