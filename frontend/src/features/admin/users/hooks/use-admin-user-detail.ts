import { useQuery } from "@tanstack/react-query";

import { getAdminUserDetail } from "@/services/admin-user-service";

export const adminUserDetailQueryKey = ["admin", "users", "detail"] as const;

export function useAdminUserDetail(userId: string | null) {
  return useQuery({
    queryKey: [...adminUserDetailQueryKey, userId],
    queryFn: () => getAdminUserDetail(userId!),
    enabled: Boolean(userId),
  });
}
