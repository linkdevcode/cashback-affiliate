export interface GenerateAffiliateLinkRequest {
  url: string;
}

export interface GenerateAffiliateLinkResponse {
  id: string;
  originalUrl: string;
  affiliateUrl: string;
  shortUrl: string;
}

export interface AffiliateLinkSummary {
  id: string;
  originalUrl: string;
  affiliateUrl: string;
  shortUrl: string | null;
  createdAt: string;
}

export interface AffiliateLinkDetail {
  id: string;
  originalUrl: string;
  affiliateUrl: string;
  shortUrl: string | null;
  sub1: string;
  campaignId: string | null;
  createdAt: string;
}

export interface AffiliateLinksListResponse {
  items: AffiliateLinkSummary[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface AffiliateLinksQueryParams {
  page?: number;
  pageSize?: number;
}
