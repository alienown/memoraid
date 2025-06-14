import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { useAuth } from "@/core/auth/useAuth";
import { validateAuthForm } from "@/core/validation/auth";

export default function Login() {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [emailError, setEmailError] = useState<string | null>(null);
  const [passwordError, setPasswordError] = useState<string | null>(
    null
  );
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
    setEmailError(null);
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
    setPasswordError(null);
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const validationResult = validateAuthForm(email, password);
    if (!validationResult.isValid) {
      setEmailError(validationResult.emailError);
      setPasswordError(validationResult.passwordError);
      return;
    }

    setIsSubmitting(true);

    const response = await login(email, password);

    if (response.isSuccess) {
      toast.success("Login successful!");
      navigate("/generate");
    } else {
      toast.error(response.error ?? "Login failed. Please try again.");
    }

    setIsSubmitting(false);
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
                  className="text-sm text-red-500"
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
                  className="text-sm text-red-500"
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
