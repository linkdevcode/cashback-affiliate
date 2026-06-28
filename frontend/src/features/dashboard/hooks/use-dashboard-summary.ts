import { useQuery } from "@tanstack/react-query";

import { getDashboardSummary } from "@/services/dashboard-service";

export const dashboardSummaryQueryKey = ["dashboard", "summary"] as const;

export function useDashboardSummary() {
  return useQuery({
    queryKey: dashboardSummaryQueryKey,
    queryFn: getDashboardSummary,
  });
}
