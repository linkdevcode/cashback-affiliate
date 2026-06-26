import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type { DashboardSummary } from "@/types/dashboard";

export async function getDashboardSummary(): Promise<DashboardSummary> {
  const response = await apiClient.get<ApiResponse<DashboardSummary>>("/dashboard");

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
