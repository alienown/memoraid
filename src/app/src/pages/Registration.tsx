import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { useAuth } from "@/core/auth/useAuth";
import { validateAuthForm } from "@/core/validation/auth";

export const Registration: React.FC = () => {
  const navigate = useNavigate();
  const { register } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [emailError, setEmailError] = useState<string | null>(null);
  const [passwordError, setPasswordError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
    setEmailError(null);
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
    setPasswordError(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const validationResult = validateAuthForm(email, password);
    if (!validationResult.isValid) {
      setEmailError(validationResult.emailError);
      setPasswordError(validationResult.passwordError);
      return;
    }

    setIsSubmitting(true);

    const response = await register(email, password);

    if (response.isSuccess) {
      toast.success("Registration successful! You have been logged in.");
      navigate("/generate");
    } else {
      toast.error(response.error ?? "Registration failed. Please try again.");
    }

    setIsSubmitting(false);
  };

  return (
    <div className="flex justify-center items-center h-full">
      <Card className="w-[350px]">
        <CardHeader>
          <CardTitle className="text-center">Create an account</CardTitle>
          <CardDescription className="text-center">
            Enter your details below to register
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <label htmlFor="email">Email</label>{" "}
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
              {isSubmitting ? "Registering..." : "Register"}
            </Button>
          </form>
        </CardContent>
        <CardFooter className="flex flex-col space-y-4">
          <div className="text-sm text-center text-muted-foreground">
            Already have an account?{" "}
            <a
              href="/login"
              className="text-primary underline-offset-4 hover:underline"
              onClick={(e) => {
                e.preventDefault();
                navigate("/login");
              }}
            >
              Log in
            </a>
          </div>
        </CardFooter>
      </Card>
    </div>
  );
};

export default Registration;
