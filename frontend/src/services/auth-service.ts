import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type { LoginResponse } from "@/types/auth";

export async function loginWithGoogle(idToken: string): Promise<LoginResponse> {
  const response = await apiClient.post<ApiResponse<LoginResponse>>(
    "/auth/google",
    { idToken },
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
