import { useQuery } from "@tanstack/react-query";

import { getAccessToken } from "@/lib/token-storage";
import { getCurrentUser } from "@/services/user-service";

export const currentUserQueryKey = ["users", "me"] as const;

export function useCurrentUser(enabled = true) {
  return useQuery({
    queryKey: currentUserQueryKey,
    queryFn: getCurrentUser,
    enabled: enabled && Boolean(getAccessToken()),
    staleTime: 5 * 60_000,
  });
}
