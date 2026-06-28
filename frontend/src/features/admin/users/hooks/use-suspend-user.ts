import { useMutation, useQueryClient } from "@tanstack/react-query";

import { suspendAdminUser } from "@/services/admin-user-service";

import { adminUserDetailQueryKey } from "./use-admin-user-detail";
import { adminUsersQueryKey } from "./use-admin-users";

export function useSuspendUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: suspendAdminUser,
    onSuccess: async (_, userId) => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: adminUsersQueryKey }),
        queryClient.invalidateQueries({
          queryKey: [...adminUserDetailQueryKey, userId],
        }),
      ]);
    },
  });
}
