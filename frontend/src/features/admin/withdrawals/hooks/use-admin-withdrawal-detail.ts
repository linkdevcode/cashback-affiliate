import { useQuery } from "@tanstack/react-query";

import { getAdminWithdrawalDetail } from "@/services/admin-withdrawal-service";

export const adminWithdrawalDetailQueryKey = ["admin", "withdrawals", "detail"] as const;

export function useAdminWithdrawalDetail(withdrawalId: string | null) {
  return useQuery({
    queryKey: [...adminWithdrawalDetailQueryKey, withdrawalId],
    queryFn: () => getAdminWithdrawalDetail(withdrawalId!),
    enabled: Boolean(withdrawalId),
  });
}
