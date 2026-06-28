import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type {
  AdminWithdrawalActionResult,
  AdminWithdrawalDetail,
  AdminWithdrawalsListResponse,
  AdminWithdrawalsQueryParams,
  RejectWithdrawalRequest,
} from "@/types/admin-withdrawal";

export async function getAdminWithdrawals(
  params: AdminWithdrawalsQueryParams = {},
): Promise<AdminWithdrawalsListResponse> {
  const response = await apiClient.get<ApiResponse<AdminWithdrawalsListResponse>>(
    "/admin/withdrawals",
    {
      params: {
        page: params.page ?? 1,
        pageSize: params.pageSize ?? 20,
        user: params.user || undefined,
        status: params.status,
      },
    },
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function getAdminWithdrawalDetail(
  id: string,
): Promise<AdminWithdrawalDetail> {
  const response = await apiClient.get<ApiResponse<AdminWithdrawalDetail>>(
    `/admin/withdrawals/${id}`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function approveAdminWithdrawal(
  id: string,
): Promise<AdminWithdrawalActionResult> {
  const response = await apiClient.post<ApiResponse<AdminWithdrawalActionResult>>(
    `/admin/withdrawals/${id}/approve`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function rejectAdminWithdrawal(
  id: string,
  request?: RejectWithdrawalRequest,
): Promise<AdminWithdrawalActionResult> {
  const response = await apiClient.post<ApiResponse<AdminWithdrawalActionResult>>(
    `/admin/withdrawals/${id}/reject`,
    request ?? {},
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function completeAdminWithdrawal(
  id: string,
): Promise<AdminWithdrawalActionResult> {
  const response = await apiClient.post<ApiResponse<AdminWithdrawalActionResult>>(
    `/admin/withdrawals/${id}/complete`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
