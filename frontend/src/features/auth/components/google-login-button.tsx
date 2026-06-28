"use client";

import { GoogleLogin, type CredentialResponse } from "@react-oauth/google";

import { Button } from "@/components/ui/button";

interface GoogleLoginButtonProps {
  onSuccess: (idToken: string) => void;
  onError?: () => void;
  disabled?: boolean;
}

export function GoogleLoginButton({
  onSuccess,
  onError,
  disabled = false,
}: GoogleLoginButtonProps) {
  const clientId = process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID;

  const handleSuccess = (response: CredentialResponse) => {
    if (!response.credential) {
      onError?.();
      return;
    }

    onSuccess(response.credential);
  };

  if (!clientId) {
    return (
      <Button className="w-full" disabled variant="outline">
        Google Client ID not configured
      </Button>
    );
  }

  return (
    <div
      className={`flex w-full justify-center [&>div]:w-full ${disabled ? "pointer-events-none opacity-50" : ""}`}
    >
      <GoogleLogin
        onSuccess={handleSuccess}
        onError={onError}
        useOneTap={false}
        theme="outline"
        size="large"
        text="signin_with"
        shape="rectangular"
        width="360"
      />
    </div>
  );
}
