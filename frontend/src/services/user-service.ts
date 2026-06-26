import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type { UserInfo } from "@/types/auth";

export async function getCurrentUser(): Promise<UserInfo> {
  const response = await apiClient.get<ApiResponse<UserInfo>>("/users/me");

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
