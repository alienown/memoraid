export interface ValidationResult {
  isValid: boolean;
  emailError: string | null;
  passwordError: string | null;
}

export function validateAuthForm(
  email: string,
  password: string
): ValidationResult {
  let isValid = true;
  let emailError: string | null = null;
  let passwordError: string | null = null;

  if (!email.trim()) {
    emailError = "Email is required";
    isValid = false;
  }

  if (!password.trim()) {
    passwordError = "Password is required";
    isValid = false;
  }

  return {
    isValid,
    emailError,
    passwordError,
  };
}
