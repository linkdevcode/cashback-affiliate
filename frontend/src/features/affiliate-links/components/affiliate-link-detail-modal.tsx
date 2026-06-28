"use client";

import { Check, Copy, Loader2 } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { useAffiliateLinkDetail } from "@/features/affiliate-links/hooks/use-affiliate-link-detail";
import { isApiError } from "@/services/api-client";

interface AffiliateLinkDetailModalProps {
  linkId: string | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AffiliateLinkDetailModal({
  linkId,
  open,
  onOpenChange,
}: AffiliateLinkDetailModalProps) {
  const { data, isLoading, isError, error } = useAffiliateLinkDetail(linkId);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogHeader>
        <DialogTitle>Affiliate link details</DialogTitle>
        <DialogDescription>
          Full information for your generated cashback link.
        </DialogDescription>
      </DialogHeader>

      <DialogContent className="space-y-4">
        {isLoading ? (
          <div className="flex items-center justify-center py-8 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading details...
          </div>
        ) : null}

        {isError ? (
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        ) : null}

        {data ? (
          <>
            <DetailField label="Original URL" value={data.originalUrl} copyable />
            <DetailField label="Affiliate URL" value={data.affiliateUrl} copyable />
            {data.shortUrl ? (
              <DetailField label="Short URL" value={data.shortUrl} copyable />
            ) : null}
            <DetailField label="Tracking ID (Sub1)" value={data.sub1} />
            {data.campaignId ? (
              <DetailField label="Campaign ID" value={data.campaignId} />
            ) : null}
            <DetailField label="Created at" value={formatDateTime(data.createdAt)} />
          </>
        ) : null}
      </DialogContent>

      <DialogFooter>
        <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
          Close
        </Button>
      </DialogFooter>
    </Dialog>
  );
}

interface DetailFieldProps {
  label: string;
  value: string;
  copyable?: boolean;
}

function DetailField({ label, value, copyable = false }: DetailFieldProps) {
  const [copied, setCopied] = useState(false);

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(value);
      setCopied(true);
      window.setTimeout(() => setCopied(false), 2000);
    } catch {
      setCopied(false);
    }
  };

  return (
    <div className="space-y-1.5">
      <div className="flex items-center justify-between gap-2">
        <p className="text-xs font-medium text-muted-foreground">{label}</p>
        {copyable ? (
          <Button type="button" variant="ghost" size="xs" onClick={handleCopy}>
            {copied ? <Check /> : <Copy />}
            {copied ? "Copied" : "Copy"}
          </Button>
        ) : null}
      </div>
      <p className="break-all rounded-lg border bg-muted/30 px-3 py-2 text-sm">{value}</p>
    </div>
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
    return error.response?.data?.message ?? "Unable to load link details.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load link details.";
}
