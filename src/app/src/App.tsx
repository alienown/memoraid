import "./App.css";
import { Toaster } from "./components/ui/sonner";
import { RouterProvider } from "react-router-dom";
import { router } from "./lib/router";

function App() {
  return (
    <>
      <RouterProvider router={router} />
      <Toaster />
    </>
  );
}

export default App;
