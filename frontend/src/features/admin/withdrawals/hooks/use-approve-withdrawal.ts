import { useMutation, useQueryClient } from "@tanstack/react-query";

import { approveAdminWithdrawal } from "@/services/admin-withdrawal-service";

import { adminWithdrawalDetailQueryKey } from "./use-admin-withdrawal-detail";
import { adminWithdrawalsQueryKey } from "./use-admin-withdrawals";

export function useApproveWithdrawal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: approveAdminWithdrawal,
    onSuccess: async (_, withdrawalId) => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: adminWithdrawalsQueryKey }),
        queryClient.invalidateQueries({
          queryKey: [...adminWithdrawalDetailQueryKey, withdrawalId],
        }),
      ]);
    },
  });
}
