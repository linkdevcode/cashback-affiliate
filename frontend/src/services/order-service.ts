import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type {
  OrderDetail,
  OrdersListResponse,
  OrdersQueryParams,
  OrderSummary,
} from "@/types/order";

export async function getOrders(
  params: OrdersQueryParams = {},
): Promise<OrdersListResponse> {
  const response = await apiClient.get<ApiResponse<OrdersListResponse>>("/orders", {
    params: {
      page: params.page ?? 1,
      pageSize: params.pageSize ?? 20,
      status: params.status,
      sortBy: params.sortBy ?? "createdAt",
      direction: params.direction ?? "desc",
    },
  });

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function getOrderDetail(id: string): Promise<OrderDetail> {
  const response = await apiClient.get<ApiResponse<OrderDetail>>(`/orders/${id}`);

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function getOrderSummary(): Promise<OrderSummary> {
  const response = await apiClient.get<ApiResponse<OrderSummary>>("/orders/summary");

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
