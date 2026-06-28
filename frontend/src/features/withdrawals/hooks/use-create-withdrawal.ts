import { useMutation } from "@tanstack/react-query";

import { createWithdrawal } from "@/services/withdrawal-service";
import type { CreateWithdrawalRequest } from "@/types/withdrawal";

export function useCreateWithdrawal() {
  return useMutation({
    mutationFn: (payload: CreateWithdrawalRequest) => createWithdrawal(payload),
  });
}
