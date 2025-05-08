import { createBrowserRouter, Navigate } from "react-router-dom";
import Generate from "../pages/generate/Generate";
import { Flashcards } from "../pages/flashcards/Flashcards";
import Registration from "../pages/Registration";

export const router = createBrowserRouter([
  {
    path: "/register",
    element: <Registration />,
  },
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
