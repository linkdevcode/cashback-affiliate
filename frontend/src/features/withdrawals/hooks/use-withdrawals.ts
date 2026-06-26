import { useQuery } from "@tanstack/react-query";

import { getWithdrawals } from "@/services/withdrawal-service";
import type { WithdrawalsQueryParams } from "@/types/withdrawal";

export const withdrawalsQueryKey = ["withdrawals", "list"] as const;

export function useWithdrawals(params: WithdrawalsQueryParams = {}) {
  const page = params.page ?? 1;
  const pageSize = params.pageSize ?? 20;
  const status = params.status;
  const sortBy = params.sortBy ?? "requestedAt";
  const direction = params.direction ?? "desc";

  return useQuery({
    queryKey: [...withdrawalsQueryKey, page, pageSize, status, sortBy, direction],
    queryFn: () =>
      getWithdrawals({
        page,
        pageSize,
        status,
        sortBy,
        direction,
      }),
  });
}
