import { useMutation, useQueryClient } from "@tanstack/react-query";

import { rejectAdminWithdrawal } from "@/services/admin-withdrawal-service";
import type { RejectWithdrawalRequest } from "@/types/admin-withdrawal";

import { adminWithdrawalDetailQueryKey } from "./use-admin-withdrawal-detail";
import { adminWithdrawalsQueryKey } from "./use-admin-withdrawals";

interface RejectWithdrawalVariables {
  withdrawalId: string;
  request?: RejectWithdrawalRequest;
}

export function useRejectWithdrawal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ withdrawalId, request }: RejectWithdrawalVariables) =>
      rejectAdminWithdrawal(withdrawalId, request),
    onSuccess: async (_, { withdrawalId }) => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: adminWithdrawalsQueryKey }),
        queryClient.invalidateQueries({
          queryKey: [...adminWithdrawalDetailQueryKey, withdrawalId],
        }),
      ]);
    },
  });
}
