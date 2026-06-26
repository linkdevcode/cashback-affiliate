import { useQuery } from "@tanstack/react-query";

import { getAdminUsers } from "@/services/admin-user-service";
import type { AdminUsersQueryParams } from "@/types/admin-user";

export const adminUsersQueryKey = ["admin", "users", "list"] as const;

export function useAdminUsers(params: AdminUsersQueryParams = {}) {
  const page = params.page ?? 1;
  const pageSize = params.pageSize ?? 20;
  const email = params.email;
  const name = params.name;
  const status = params.status;

  return useQuery({
    queryKey: [...adminUsersQueryKey, page, pageSize, email, name, status],
    queryFn: () =>
      getAdminUsers({
        page,
        pageSize,
        email,
        name,
        status,
      }),
  });
}
