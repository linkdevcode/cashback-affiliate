import { useQuery } from "@tanstack/react-query";

import { getOrderDetail } from "@/services/order-service";

export const orderDetailQueryKey = ["orders", "detail"] as const;

export function useOrderDetail(orderId: string | null) {
  return useQuery({
    queryKey: [...orderDetailQueryKey, orderId],
    queryFn: () => getOrderDetail(orderId!),
    enabled: Boolean(orderId),
  });
}
