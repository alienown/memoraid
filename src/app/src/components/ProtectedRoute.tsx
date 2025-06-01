import { useAuth } from "@/core/auth/useAuth";
import { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { FullPageLoader } from "./FullPageLoader";

interface ProtectedRouteProps {
  children: ReactNode;
}

export function ProtectedRoute({ children }: ProtectedRouteProps) {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated === null) {
    return <FullPageLoader delay={1000} />
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
}
