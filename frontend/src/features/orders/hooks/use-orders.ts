import { useQuery } from "@tanstack/react-query";

import { getOrders } from "@/services/order-service";
import type { OrdersQueryParams } from "@/types/order";

export const ordersQueryKey = ["orders", "list"] as const;

export function useOrders(params: OrdersQueryParams = {}) {
  const page = params.page ?? 1;
  const pageSize = params.pageSize ?? 20;
  const status = params.status;
  const sortBy = params.sortBy ?? "createdAt";
  const direction = params.direction ?? "desc";

  return useQuery({
    queryKey: [...ordersQueryKey, page, pageSize, status, sortBy, direction],
    queryFn: () =>
      getOrders({
        page,
        pageSize,
        status,
        sortBy,
        direction,
      }),
  });
}
