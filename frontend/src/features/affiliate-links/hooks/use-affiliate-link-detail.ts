import { useQuery } from "@tanstack/react-query";

import { getAffiliateLinkDetail } from "@/services/affiliate-link-service";

export const affiliateLinkDetailQueryKey = ["affiliate-links", "detail"] as const;

export function useAffiliateLinkDetail(id: string | null) {
  return useQuery({
    queryKey: [...affiliateLinkDetailQueryKey, id],
    queryFn: () => getAffiliateLinkDetail(id!),
    enabled: Boolean(id),
  });
}
