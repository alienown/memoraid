import { useContext } from "react";
import { AuthContext } from "./AuthContext";

export const useAuth = () => {
  const auth = useContext(AuthContext);

  return auth;
};
