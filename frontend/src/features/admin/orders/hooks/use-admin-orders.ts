import { useQuery } from "@tanstack/react-query";

import { getAdminOrders } from "@/services/admin-order-service";
import type { AdminOrdersQueryParams } from "@/types/admin-order";

export const adminOrdersQueryKey = ["admin", "orders", "list"] as const;

export function useAdminOrders(params: AdminOrdersQueryParams = {}) {
  const page = params.page ?? 1;
  const pageSize = params.pageSize ?? 20;
  const orderId = params.orderId;
  const user = params.user;
  const status = params.status;
  const fromDate = params.fromDate;
  const toDate = params.toDate;

  return useQuery({
    queryKey: [
      ...adminOrdersQueryKey,
      page,
      pageSize,
      orderId,
      user,
      status,
      fromDate,
      toDate,
    ],
    queryFn: () =>
      getAdminOrders({
        page,
        pageSize,
        orderId,
        user,
        status,
        fromDate,
        toDate,
      }),
  });
}
