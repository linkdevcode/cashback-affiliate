import { useQuery } from "@tanstack/react-query";

import { getAffiliateLinks } from "@/services/affiliate-link-service";
import type { AffiliateLinksQueryParams } from "@/types/affiliate-link";

export const affiliateLinksQueryKey = ["affiliate-links", "list"] as const;

export function useAffiliateLinks(params: AffiliateLinksQueryParams = {}) {
  const page = params.page ?? 1;
  const pageSize = params.pageSize ?? 20;

  return useQuery({
    queryKey: [...affiliateLinksQueryKey, page, pageSize],
    queryFn: () => getAffiliateLinks({ page, pageSize }),
  });
}
