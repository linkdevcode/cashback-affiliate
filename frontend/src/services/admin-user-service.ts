import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type {
  AdminUserActionResult,
  AdminUserDetail,
  AdminUsersListResponse,
  AdminUsersQueryParams,
} from "@/types/admin-user";

export async function getAdminUsers(
  params: AdminUsersQueryParams = {},
): Promise<AdminUsersListResponse> {
  const response = await apiClient.get<ApiResponse<AdminUsersListResponse>>(
    "/admin/users",
    {
      params: {
        page: params.page ?? 1,
        pageSize: params.pageSize ?? 20,
        email: params.email || undefined,
        name: params.name || undefined,
        status: params.status,
      },
    },
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function getAdminUserDetail(id: string): Promise<AdminUserDetail> {
  const response = await apiClient.get<ApiResponse<AdminUserDetail>>(
    `/admin/users/${id}`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function suspendAdminUser(id: string): Promise<AdminUserActionResult> {
  const response = await apiClient.post<ApiResponse<AdminUserActionResult>>(
    `/admin/users/${id}/suspend`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function activateAdminUser(id: string): Promise<AdminUserActionResult> {
  const response = await apiClient.post<ApiResponse<AdminUserActionResult>>(
    `/admin/users/${id}/activate`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
