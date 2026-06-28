import { apiClient } from "@/services/api-client";
import type { ApiResponse } from "@/types/api";
import type {
  AffiliateLinkDetail,
  AffiliateLinksListResponse,
  AffiliateLinksQueryParams,
  GenerateAffiliateLinkRequest,
  GenerateAffiliateLinkResponse,
} from "@/types/affiliate-link";

export async function generateAffiliateLink(
  request: GenerateAffiliateLinkRequest,
): Promise<GenerateAffiliateLinkResponse> {
  const response = await apiClient.post<ApiResponse<GenerateAffiliateLinkResponse>>(
    "/affiliate-links",
    request,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function getAffiliateLinks(
  params: AffiliateLinksQueryParams = {},
): Promise<AffiliateLinksListResponse> {
  const response = await apiClient.get<ApiResponse<AffiliateLinksListResponse>>(
    "/affiliate-links",
    {
      params: {
        page: params.page ?? 1,
        pageSize: params.pageSize ?? 20,
      },
    },
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export async function getAffiliateLinkDetail(id: string): Promise<AffiliateLinkDetail> {
  const response = await apiClient.get<ApiResponse<AffiliateLinkDetail>>(
    `/affiliate-links/${id}`,
  );

  if (!response.data.success) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}
