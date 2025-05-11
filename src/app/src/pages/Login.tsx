import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useNavigate } from "react-router-dom";
import { LoginUserRequest } from "@/api/api";
import { Api } from "@/api/api";
import { toast } from "sonner";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

const apiClient = new Api();

export default function Login() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [emailError, setEmailError] = useState<string | undefined>(undefined);
  const [passwordError, setPasswordError] = useState<string | undefined>(
    undefined
  );
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Handlers
  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
    setEmailError(undefined);
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
    setPasswordError(undefined);
  };

  const validateForm = (): boolean => {
    let isValid = true;

    if (!email.trim()) {
      setEmailError("Email is required");
      isValid = false;
    }

    if (!password.trim()) {
      setPasswordError("Password is required");
      isValid = false;
    }

    return isValid;
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    setIsSubmitting(true);

    try {
      const request: LoginUserRequest = { email, password };
      const response = await apiClient.users.loginUser(request);
      if (response.data.isSuccess) {
        toast.success("Login successful!");
        navigate("/generate");
      } else {
        if (response.data.errors) {
          response.data.errors.forEach((error) => {
            if (error.propertyName === "email") {
              setEmailError(error.message);
            } else if (error.propertyName === "password") {
              setPasswordError(error.message);
            } else {
              toast.error(error.message);
            }
          });
        }
      }
    } catch (error: unknown) {
      const apiError = error as
        | { error?: { errors?: { message: string }[] } }
        | undefined;
      toast.error(
        apiError?.error?.errors?.[0]?.message ||
          "An error occurred during login"
      );
    } finally {
      setIsSubmitting(false);
    }
  };
  return (
    <div className="flex justify-center items-center h-full">
      <Card className="w-[350px]">
        <CardHeader>
          <CardTitle className="text-center">Login to Memoraid</CardTitle>
          <CardDescription className="text-center">
            Enter your credentials to access your account
          </CardDescription>
        </CardHeader>

        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <label htmlFor="email">Email</label>
              <Input
                id="email"
                type="email"
                value={email}
                onChange={handleEmailChange}
                disabled={isSubmitting}
              />
              {emailError && (
                <p
                  id="email-error"
                  className="text-sm font-medium text-red-500"
                >
                  {emailError}
                </p>
              )}
            </div>

            <div className="space-y-2">
              <label htmlFor="password">Password</label>
              <Input
                id="password"
                type="password"
                value={password}
                onChange={handlePasswordChange}
                disabled={isSubmitting}
              />
              {passwordError && (
                <p
                  id="password-error"
                  className="text-sm font-medium text-red-500"
                >
                  {passwordError}
                </p>
              )}
            </div>

            <Button type="submit" className="w-full" disabled={isSubmitting}>
              {isSubmitting ? "Logging in..." : "Login"}
            </Button>
          </form>
        </CardContent>

        <CardFooter className="flex flex-col space-y-4">
          <div className="text-sm text-center text-muted-foreground">
            Don't have an account?{" "}
            <a
              href="/register"
              className="text-primary underline-offset-4 hover:underline"
            >
              Register
            </a>
          </div>
        </CardFooter>
      </Card>
    </div>
  );
}
