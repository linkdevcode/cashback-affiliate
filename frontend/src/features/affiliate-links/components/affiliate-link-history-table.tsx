"use client";

import { Eye, Loader2 } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { AffiliateLinkDetailModal } from "@/features/affiliate-links/components/affiliate-link-detail-modal";
import { AffiliateLinkPagination } from "@/features/affiliate-links/components/affiliate-link-pagination";
import { useAffiliateLinks } from "@/features/affiliate-links/hooks/use-affiliate-links";
import { isApiError } from "@/services/api-client";

const PAGE_SIZE = 20;

export function AffiliateLinkHistoryTable() {
  const [page, setPage] = useState(1);
  const [selectedLinkId, setSelectedLinkId] = useState<string | null>(null);
  const [isDetailOpen, setIsDetailOpen] = useState(false);

  const { data, isLoading, isError, error, isFetching } = useAffiliateLinks({
    page,
    pageSize: PAGE_SIZE,
  });

  const handleViewDetail = (linkId: string) => {
    setSelectedLinkId(linkId);
    setIsDetailOpen(true);
  };

  const handleDetailOpenChange = (open: boolean) => {
    setIsDetailOpen(open);

    if (!open) {
      setSelectedLinkId(null);
    }
  };

  return (
    <>
      <Card>
        <CardHeader>
          <CardTitle>Generated links</CardTitle>
          <CardDescription>
            Your previously created affiliate links and tracking URLs.
          </CardDescription>
        </CardHeader>

        <CardContent className="space-y-4">
          {isLoading ? (
            <div className="flex items-center justify-center py-12 text-sm text-muted-foreground">
              <Loader2 className="mr-2 size-4 animate-spin" />
              Loading link history...
            </div>
          ) : null}

          {isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(error)}
            </p>
          ) : null}

          {!isLoading && !isError && data?.items.length === 0 ? (
            <p className="py-12 text-center text-sm text-muted-foreground">
              No affiliate links yet. Generate your first cashback link to see it here.
            </p>
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <div className="overflow-x-auto rounded-lg border">
              <table className="w-full min-w-[640px] text-left text-sm">
                <thead className="border-b bg-muted/40">
                  <tr>
                    <th className="px-4 py-3 font-medium">Original URL</th>
                    <th className="px-4 py-3 font-medium">Affiliate URL</th>
                    <th className="px-4 py-3 font-medium">Created at</th>
                    <th className="px-4 py-3 font-medium text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {data.items.map((link) => (
                    <tr key={link.id} className="border-b last:border-b-0">
                      <td className="max-w-[220px] px-4 py-3">
                        <UrlCell value={link.originalUrl} />
                      </td>
                      <td className="max-w-[220px] px-4 py-3">
                        <UrlCell value={link.affiliateUrl} />
                      </td>
                      <td className="whitespace-nowrap px-4 py-3 text-muted-foreground">
                        {formatDateTime(link.createdAt)}
                      </td>
                      <td className="px-4 py-3 text-right">
                        <Button
                          type="button"
                          variant="outline"
                          size="sm"
                          onClick={() => handleViewDetail(link.id)}
                        >
                          <Eye />
                          View
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : null}

          {!isLoading && !isError && data && data.totalCount > 0 ? (
            <AffiliateLinkPagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
              disabled={isFetching}
            />
          ) : null}
        </CardContent>
      </Card>

      <AffiliateLinkDetailModal
        linkId={selectedLinkId}
        open={isDetailOpen}
        onOpenChange={handleDetailOpenChange}
      />
    </>
  );
}

function UrlCell({ value }: { value: string }) {
  return (
    <span className="block truncate" title={value}>
      {value}
    </span>
  );
}

function formatDateTime(value: string): string {
  return new Intl.DateTimeFormat("vi-VN", {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(value));
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load affiliate link history.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load affiliate link history.";
}
