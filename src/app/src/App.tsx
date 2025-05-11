import "./App.css";
import { Toaster } from "./components/ui/sonner";
import { RouterProvider } from "react-router-dom";
import { router } from "./lib/router";
import { AuthProvider } from "./lib/auth/AuthProvider";
import { AuthHandlerWrapper } from "./lib/auth/AuthHandlerWrapper";

function App() {
  return (
    <AuthProvider>
      <AuthHandlerWrapper>
        <RouterProvider router={router} />
        <Toaster />
      </AuthHandlerWrapper>
    </AuthProvider>
  );
}

export default App;
