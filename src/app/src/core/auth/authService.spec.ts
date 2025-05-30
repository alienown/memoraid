import { FirebaseError } from "firebase/app";
import { handleAuthError } from "./authService";

describe("authService", () => {
  describe("handleAuthError", () => {
    it("should return generic error message when error is not a FirebaseError", () => {
      // Arrange
      const error = new Error("Some random error");

      // Act
      const result = handleAuthError(error);

      // Assert
      expect(result.isSuccess).toBe(false);
      expect(result.error).toBe("Something went wrong");
    });

    describe("Handling Firebase errors", () => {
      it.each([
        {
          errorCode: "auth/user-not-found",
          expectedMessage: "Invalid email or password",
          description:
            "should return invalid credentials message when auth/user-not-found error occurs",
        },
        {
          errorCode: "auth/email-already-in-use",
          expectedMessage: "Email is already in use",
          description:
            "should return email in use message when auth/email-already-in-use error occurs",
        },
        {
          errorCode: "auth/email-already-exists",
          expectedMessage: "Email is already in use",
          description:
            "should return email in use message when auth/email-already-exists error occurs",
        },
        {
          errorCode: "auth/weak-password",
          expectedMessage: "Password should be at least 6 characters",
          description:
            "should return weak password message when auth/weak-password error occurs",
        },
        {
          errorCode: "auth/unknown-error-code",
          expectedMessage: "Something went wrong",
          description:
            "should return generic error message when unknown Firebase error code is provided",
        },
      ])("$description", ({ errorCode, expectedMessage }) => {
        // Arrange
        const error = new FirebaseError(errorCode, "Firebase error message");

        // Act
        const result = handleAuthError(error);

        // Assert
        expect(result.isSuccess).toBe(false);
        expect(result.error).toBe(expectedMessage);
      });
    });
  });
});
