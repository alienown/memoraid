import { ReactNode, useState } from "react";
import { tokenStorage } from "../tokenStorage";
import { AuthContext } from "./AuthContext";

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(
    tokenStorage.hasToken()
  );

  const login = (token: string) => {
    tokenStorage.setToken(token);
    setIsAuthenticated(true);
  };

  const logout = () => {
    tokenStorage.removeToken();
    setIsAuthenticated(false);
  };

  const value = {
    isAuthenticated,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
