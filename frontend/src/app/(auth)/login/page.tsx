"use client";

import { useRouter } from "next/navigation";
import { useState } from "react";

import { PublicRoute } from "@/components/routes/public-route";
import { GoogleLoginButton } from "@/features/auth/components/google-login-button";
import { useAuth } from "@/providers/auth-provider";
import { isApiError } from "@/services/api-client";

export default function LoginPage() {
  const router = useRouter();
  const { loginWithGoogle } = useAuth();
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleGoogleSuccess = async (idToken: string) => {
    setErrorMessage(null);
    setIsSubmitting(true);

    try {
      await loginWithGoogle(idToken);
      router.replace("/dashboard");
    } catch (error) {
      if (isApiError(error)) {
        setErrorMessage(
          error.response?.data?.message ?? "Unable to sign in with Google.",
        );
        return;
      }

      if (error instanceof Error) {
        setErrorMessage(error.message);
        return;
      }

      setErrorMessage("Unable to sign in with Google.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <PublicRoute>
      <div className="space-y-4">
        <p className="text-center text-sm text-muted-foreground">
          Sign in with your Google account to access cashback features.
        </p>

        <GoogleLoginButton
          onSuccess={handleGoogleSuccess}
          onError={() => setErrorMessage("Google sign-in was cancelled or failed.")}
          disabled={isSubmitting}
        />

        {isSubmitting ? (
          <p className="text-center text-sm text-muted-foreground">
            Signing in...
          </p>
        ) : null}

        {errorMessage ? (
          <p className="text-center text-sm text-destructive" role="alert">
            {errorMessage}
          </p>
        ) : null}
      </div>
    </PublicRoute>
  );
}
