import { createBrowserRouter, Navigate } from "react-router-dom";
import Generate from "../pages/generate/Generate";
import { Flashcards } from "../pages/flashcards/Flashcards";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Navigate to="/generate" replace />,
  },
  {
    path: "/generate",
    element: <Generate />,
  },
  {
    path: "/flashcards",
    element: <Flashcards />,
  },
]);