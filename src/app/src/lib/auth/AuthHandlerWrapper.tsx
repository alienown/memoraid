import { ReactNode, useEffect } from "react";
import { useAuth } from "./useAuth";
import { setupUnauthenticatedHandler } from "../axiosInterceptor";

interface AuthHandlerWrapperProps {
  children: ReactNode;
}

export function AuthHandlerWrapper({ children }: AuthHandlerWrapperProps) {
  const { logout } = useAuth();

  useEffect(() => {
    setupUnauthenticatedHandler(() => {
      logout();
    });

  }, [logout]);

  return <>{children}</>;
}
