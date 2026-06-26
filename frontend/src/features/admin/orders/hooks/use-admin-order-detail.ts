import { useQuery } from "@tanstack/react-query";

import { getAdminOrderDetail } from "@/services/admin-order-service";

export const adminOrderDetailQueryKey = ["admin", "orders", "detail"] as const;

export function useAdminOrderDetail(orderId: string | null) {
  return useQuery({
    queryKey: [...adminOrderDetailQueryKey, orderId],
    queryFn: () => getAdminOrderDetail(orderId!),
    enabled: Boolean(orderId),
  });
}
