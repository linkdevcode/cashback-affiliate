import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type { AdminDashboardSummary } from "@/types/admin-dashboard";

export async function getAdminDashboardSummary(): Promise<AdminDashboardSummary> {
  const response = await apiClient.get<ApiResponse<AdminDashboardSummary>>(
    "/admin/dashboard",
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
