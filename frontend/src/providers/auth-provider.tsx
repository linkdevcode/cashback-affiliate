"use client";

import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";

import {
  clearTokens,
  getAccessToken,
  hasSession,
  setStoredUser,
  setTokens,
} from "@/lib/token-storage";
import { loginWithGoogle as loginWithGoogleApi } from "@/services/auth-service";
import { getCurrentUser } from "@/services/user-service";
import type { UserInfo } from "@/types/auth";

interface AuthContextValue {
  user: UserInfo | null;
  isLoading: boolean;
  isAuthenticated: boolean;
  loginWithGoogle: (idToken: string) => Promise<void>;
  logout: () => void;
  refreshCurrentUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | null>(null);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<UserInfo | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const refreshCurrentUser = useCallback(async () => {
    if (!getAccessToken()) {
      setUser(null);
      return;
    }

    const currentUser = await getCurrentUser();
    setStoredUser(currentUser);
    setUser(currentUser);
  }, []);

  const initializeSession = useCallback(async () => {
    if (!hasSession()) {
      setUser(null);
      setIsLoading(false);
      return;
    }

    try {
      await refreshCurrentUser();
    } catch {
      clearTokens();
      setUser(null);
    } finally {
      setIsLoading(false);
    }
  }, [refreshCurrentUser]);

  useEffect(() => {
    void initializeSession();

    const handleSessionExpired = () => {
      setUser(null);
    };

    window.addEventListener("auth:session-expired", handleSessionExpired);

    return () => {
      window.removeEventListener("auth:session-expired", handleSessionExpired);
    };
  }, [initializeSession]);

  const loginWithGoogle = useCallback(
    async (idToken: string) => {
      const response = await loginWithGoogleApi(idToken);
      setTokens(
        response.accessToken,
        response.refreshToken,
        {
          id: response.user.id,
          email: response.user.email,
          fullName: response.user.fullName,
          avatarUrl: null,
          role: response.user.role,
          status: 1,
        },
      );

      await refreshCurrentUser();
    },
    [refreshCurrentUser],
  );

  const logout = useCallback(() => {
    clearTokens();
    setUser(null);
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isLoading,
      isAuthenticated: Boolean(user),
      loginWithGoogle,
      logout,
      refreshCurrentUser,
    }),
    [user, isLoading, loginWithGoogle, logout, refreshCurrentUser],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider.");
  }

  return context;
}
