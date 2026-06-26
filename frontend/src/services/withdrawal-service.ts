import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type {
  CreateWithdrawalRequest,
  CreateWithdrawalResult,
  WithdrawalsListResponse,
  WithdrawalsQueryParams,
} from "@/types/withdrawal";

export async function getWithdrawals(
  params: WithdrawalsQueryParams = {},
): Promise<WithdrawalsListResponse> {
  const response = await apiClient.get<ApiResponse<WithdrawalsListResponse>>("/withdrawals", {
    params: {
      page: params.page ?? 1,
      pageSize: params.pageSize ?? 20,
      status: params.status,
      sortBy: params.sortBy ?? "requestedAt",
      direction: params.direction ?? "desc",
    },
  });

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function createWithdrawal(
  payload: CreateWithdrawalRequest,
): Promise<CreateWithdrawalResult> {
  const response = await apiClient.post<ApiResponse<CreateWithdrawalResult>>(
    "/withdrawals",
    payload,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
