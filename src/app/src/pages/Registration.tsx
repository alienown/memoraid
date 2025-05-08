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
import { Api } from "../api/api";

export const Registration: React.FC = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [emailError, setEmailError] = useState<string | null>(null);
  const [passwordError, setPasswordError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const api = new Api();

  const validateForm = (): boolean => {
    let isValid = true;

    // Validate email
    if (!email) {
      setEmailError("Email is required");
      isValid = false;
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      setEmailError("Please enter a valid email address");
      isValid = false;
    } else {
      setEmailError(null);
    }

    // Validate password
    if (!password) {
      setPasswordError("Password is required");
      isValid = false;
    } else {
      setPasswordError(null);
    }

    return isValid;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    setIsSubmitting(true);

    try {
      const response = await api.users.registerUser({
        email,
        password,
      });

      if (response.data.isSuccess) {
        toast.success("Registration successful! Please log in.");
        navigate("/login");
      } else {
        const errors = response.data.errors;
        if (errors && errors.length > 0) {
          errors.forEach((error) => {
            if (error.propertyName === "email") {
              setEmailError(error.message);
            } else if (error.propertyName === "password") {
              setPasswordError(error.message);
            } else {
              toast.error(error.message);
            }
          });
        } else {
          toast.error("Registration failed. Please try again.");
        }
      }
    } catch (error) {
      toast.error("Failed to connect to the server. Please try again later.");
      console.error("Registration error:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen">
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
              <label htmlFor="email">Email</label>
              <Input
                id="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                disabled={isSubmitting}
                aria-describedby={emailError ? "email-error" : undefined}
                aria-invalid={!!emailError}
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
                onChange={(e) => setPassword(e.target.value)}
                disabled={isSubmitting}
                aria-describedby={passwordError ? "password-error" : undefined}
                aria-invalid={!!passwordError}
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
