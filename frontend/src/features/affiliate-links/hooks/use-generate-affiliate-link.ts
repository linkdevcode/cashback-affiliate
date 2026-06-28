import { useMutation } from "@tanstack/react-query";

import { generateAffiliateLink } from "@/services/affiliate-link-service";

export function useGenerateAffiliateLink() {
  return useMutation({
    mutationFn: generateAffiliateLink,
  });
}
