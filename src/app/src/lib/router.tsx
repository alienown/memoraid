import { createBrowserRouter, Navigate } from "react-router-dom";
import Generate from "../pages/generate/Generate";
import { Flashcards } from "../pages/flashcards/Flashcards";
import Registration from "../pages/Registration";
import Login from "../pages/Login";
import { RootLayout } from "../components/RootLayout";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />,
    children: [
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
      {
        path: "/login",
        element: <Login />,
      },
      {
        path: "/register",
        element: <Registration />,
      },
    ],
  },
]);
