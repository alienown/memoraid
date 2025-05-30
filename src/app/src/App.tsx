import "./App.css";
import { Toaster } from "./components/ui/sonner";
import { RouterProvider } from "react-router-dom";
import { router } from "./core/router";
import { AuthProvider } from "./components/AuthProvider";

function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
      <Toaster />
    </AuthProvider>
  );
}

export default App;
