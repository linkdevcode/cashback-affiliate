import { useMutation, useQueryClient } from "@tanstack/react-query";

import { activateAdminUser } from "@/services/admin-user-service";

import { adminUserDetailQueryKey } from "./use-admin-user-detail";
import { adminUsersQueryKey } from "./use-admin-users";

export function useActivateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: activateAdminUser,
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
