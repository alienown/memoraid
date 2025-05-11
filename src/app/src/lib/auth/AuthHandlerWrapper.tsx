import { ReactNode, useEffect } from "react";
import { useAuth } from "./useAuth";
import { setupForbiddenHandler } from "../axiosInterceptor";

interface AuthHandlerWrapperProps {
  children: ReactNode;
}

export function AuthHandlerWrapper({ children }: AuthHandlerWrapperProps) {
  const { logout } = useAuth();

  useEffect(() => {
    setupForbiddenHandler(() => {
      logout();
    });

  }, [logout]);

  return <>{children}</>;
}
