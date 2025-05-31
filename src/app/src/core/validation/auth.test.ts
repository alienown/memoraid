import { describe, it, expect } from "vitest";
import { validateAuthForm } from "./auth";

describe("validateAuthForm", () => {
  it("should return valid when email and password are provided", () => {
    // Arrange
    const email = "test@example.com";
    const password = "password123";

    // Act
    const result = validateAuthForm(email, password);

    // Assert
    expect(result.isValid).toBe(true);
    expect(result.emailError).toBeNull();
    expect(result.passwordError).toBeNull();
  });

  it("should return invalid when email is empty", () => {
    // Arrange
    const email = "";
    const password = "password123";

    // Act
    const result = validateAuthForm(email, password);

    // Assert
    expect(result.isValid).toBe(false);
    expect(result.emailError).toBe("Email is required");
    expect(result.passwordError).toBeNull();
  });

  it("should return invalid when email is whitespace", () => {
    // Arrange
    const email = "  ";
    const password = "password123";

    // Act
    const result = validateAuthForm(email, password);

    // Assert
    expect(result.isValid).toBe(false);
    expect(result.emailError).toBe("Email is required");
    expect(result.passwordError).toBeNull();
  });

  it("should return invalid when password is empty", () => {
    // Arrange
    const email = "test@example.com";
    const password = "";

    // Act
    const result = validateAuthForm(email, password);

    // Assert
    expect(result.isValid).toBe(false);
    expect(result.emailError).toBeNull();
    expect(result.passwordError).toBe("Password is required");
  });

  it("should return invalid when password is whitespace", () => {
    // Arrange
    const email = "test@example.com";
    const password = "  ";

    // Act
    const result = validateAuthForm(email, password);

    // Assert
    expect(result.isValid).toBe(false);
    expect(result.emailError).toBeNull();
    expect(result.passwordError).toBe("Password is required");
  });

  it("should return multiple errors when both email and password are empty", () => {
    // Arrange
    const email = "";
    const password = "";

    // Act
    const result = validateAuthForm(email, password);

    // Assert
    expect(result.isValid).toBe(false);
    expect(result.emailError).toBe("Email is required");
    expect(result.passwordError).toBe("Password is required");
  });
});
