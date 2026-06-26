import { useQuery } from "@tanstack/react-query";

import { getOrderSummary } from "@/services/order-service";

export const orderSummaryQueryKey = ["orders", "summary"] as const;

export function useOrderSummary() {
  return useQuery({
    queryKey: orderSummaryQueryKey,
    queryFn: getOrderSummary,
  });
}
