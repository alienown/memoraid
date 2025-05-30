import { render, screen } from "@testing-library/react";
import { ProtectedRoute } from "./ProtectedRoute";
import {
  AuthContext,
  createDefaultAuthContext,
} from "@/core/auth/AuthContext";
import { RouterProvider, createMemoryRouter } from "react-router";

describe("ProtectedRoute", () => {
  const renderWithAuth = (isAuthenticated: boolean) => {
    const authContextValue = {
      ...createDefaultAuthContext(),
      isAuthenticated,
    };
  
    const routes = [
      {
        path: "/login",
        element: <div>Login Page</div>,
      },
      {
        path: "/protected",
        element: (
          <ProtectedRoute>
            <div>Protected Content</div>
          </ProtectedRoute>
        ),
      },
    ];

    const router = createMemoryRouter(routes, {
      initialEntries: ["/protected"],
    });

    return render(
      <AuthContext.Provider value={authContextValue}>
        <RouterProvider router={router} />
      </AuthContext.Provider>
    );
  };

  it("should render children when user is authenticated", () => {
    // Arrange & Act
    renderWithAuth(true);

    // Assert
    expect(screen.getByText("Protected Content")).toBeVisible();
    expect(screen.queryByText("Login Page")).not.toBeInTheDocument();
  });

  it("should redirect to login page when user is not authenticated", async () => {
    // Arrange & Act
    renderWithAuth(false);

    // Assert
    expect(screen.getByText("Login Page")).toBeVisible();
    expect(screen.queryByText("Protected Content")).not.toBeInTheDocument();
  });
});
