import axios, {
  type AxiosError,
  type AxiosInstance,
  type InternalAxiosRequestConfig,
} from "axios";

import {
  clearTokens,
  getAccessToken,
  getRefreshToken,
  setAccessToken,
} from "@/lib/token-storage";
import type { ApiResponse, ApiErrorResponse } from "@/types/api";
import type { RefreshTokenResponse } from "@/types/auth";

const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5050/api/v1";

interface RetryableRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean;
}

export const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 30_000,
});

let isRefreshing = false;
let refreshQueue: Array<{
  resolve: (token: string) => void;
  reject: (error: unknown) => void;
}> = [];

function isAuthEndpoint(url?: string): boolean {
  return Boolean(
    url?.includes("/auth/google") || url?.includes("/auth/refresh-token"),
  );
}

function processRefreshQueue(error: unknown, token: string | null): void {
  refreshQueue.forEach((pending) => {
    if (error) {
      pending.reject(error);
      return;
    }

    if (token) {
      pending.resolve(token);
    }
  });

  refreshQueue = [];
}

function handleSessionExpired(): void {
  clearTokens();

  if (typeof window !== "undefined") {
    window.dispatchEvent(new Event("auth:session-expired"));

    if (!window.location.pathname.startsWith("/login")) {
      window.location.assign("/login");
    }
  }
}

async function requestTokenRefresh(refreshToken: string): Promise<string> {
  const response = await axios.post<ApiResponse<RefreshTokenResponse>>(
    `${API_BASE_URL}/auth/refresh-token`,
    { refreshToken },
    {
      headers: { "Content-Type": "application/json" },
      timeout: 30_000,
    },
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data.accessToken;
}

apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = getAccessToken();

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error: AxiosError) => Promise.reject(error),
);

apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError<ApiErrorResponse>) => {
    const originalRequest = error.config as RetryableRequestConfig | undefined;

    if (
      error.response?.status !== 401 ||
      !originalRequest ||
      originalRequest._retry ||
      isAuthEndpoint(originalRequest.url)
    ) {
      return Promise.reject(error);
    }

    const refreshToken = getRefreshToken();

    if (!refreshToken) {
      handleSessionExpired();
      return Promise.reject(error);
    }

    if (isRefreshing) {
      return new Promise<string>((resolve, reject) => {
        refreshQueue.push({ resolve, reject });
      }).then((token) => {
        originalRequest.headers.Authorization = `Bearer ${token}`;
        return apiClient(originalRequest);
      });
    }

    originalRequest._retry = true;
    isRefreshing = true;

    try {
      const refreshedAccessToken = await requestTokenRefresh(refreshToken);
      setAccessToken(refreshedAccessToken);
      processRefreshQueue(null, refreshedAccessToken);
      originalRequest.headers.Authorization = `Bearer ${refreshedAccessToken}`;
      return apiClient(originalRequest);
    } catch (refreshError) {
      processRefreshQueue(refreshError, null);
      handleSessionExpired();
      return Promise.reject(refreshError);
    } finally {
      isRefreshing = false;
    }
  },
);

export function isApiError(error: unknown): error is AxiosError<ApiErrorResponse> {
  return axios.isAxiosError(error);
}
