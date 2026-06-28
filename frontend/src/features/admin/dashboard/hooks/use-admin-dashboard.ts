import { useQuery } from "@tanstack/react-query";

import { getAdminDashboardSummary } from "@/services/admin-dashboard-service";

export const adminDashboardQueryKey = ["admin", "dashboard"] as const;

export function useAdminDashboard() {
  return useQuery({
    queryKey: adminDashboardQueryKey,
    queryFn: getAdminDashboardSummary,
  });
}
