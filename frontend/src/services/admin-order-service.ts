import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type {
  AdminOrderDetail,
  AdminOrdersListResponse,
  AdminOrdersQueryParams,
} from "@/types/admin-order";

export async function getAdminOrders(
  params: AdminOrdersQueryParams = {},
): Promise<AdminOrdersListResponse> {
  const response = await apiClient.get<ApiResponse<AdminOrdersListResponse>>(
    "/admin/orders",
    {
      params: {
        page: params.page ?? 1,
        pageSize: params.pageSize ?? 20,
        orderId: params.orderId || undefined,
        user: params.user || undefined,
        status: params.status,
        fromDate: params.fromDate || undefined,
        toDate: params.toDate || undefined,
      },
    },
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function getAdminOrderDetail(id: string): Promise<AdminOrderDetail> {
  const response = await apiClient.get<ApiResponse<AdminOrderDetail>>(
    `/admin/orders/${id}`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
