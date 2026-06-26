import { useQuery } from "@tanstack/react-query";

import { getAdminWithdrawals } from "@/services/admin-withdrawal-service";
import type { AdminWithdrawalsQueryParams } from "@/types/admin-withdrawal";

export const adminWithdrawalsQueryKey = ["admin", "withdrawals", "list"] as const;

export function useAdminWithdrawals(params: AdminWithdrawalsQueryParams = {}) {
  const page = params.page ?? 1;
  const pageSize = params.pageSize ?? 20;
  const user = params.user;
  const status = params.status;

  return useQuery({
    queryKey: [...adminWithdrawalsQueryKey, page, pageSize, user, status],
    queryFn: () =>
      getAdminWithdrawals({
        page,
        pageSize,
        user,
        status,
      }),
  });
}
