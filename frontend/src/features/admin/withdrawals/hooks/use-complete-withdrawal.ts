import { useMutation, useQueryClient } from "@tanstack/react-query";

import { completeAdminWithdrawal } from "@/services/admin-withdrawal-service";

import { adminWithdrawalDetailQueryKey } from "./use-admin-withdrawal-detail";
import { adminWithdrawalsQueryKey } from "./use-admin-withdrawals";

export function useCompleteWithdrawal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: completeAdminWithdrawal,
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
