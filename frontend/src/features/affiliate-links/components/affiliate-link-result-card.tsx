"use client";

import { Check, Copy } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import type { GenerateAffiliateLinkResponse } from "@/types/affiliate-link";

interface AffiliateLinkResultCardProps {
  result: GenerateAffiliateLinkResponse;
}

export function AffiliateLinkResultCard({ result }: AffiliateLinkResultCardProps) {
  const [copied, setCopied] = useState(false);
  const shareUrl = result.shortUrl || result.affiliateUrl;

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(shareUrl);
      setCopied(true);
      window.setTimeout(() => setCopied(false), 2000);
    } catch {
      setCopied(false);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Your cashback link is ready</CardTitle>
        <CardDescription>
          Share this link to earn cashback when someone makes a purchase.
        </CardDescription>
      </CardHeader>

      <CardContent className="space-y-4">
        <ResultField label="Original URL" value={result.originalUrl} />
        <ResultField label="Affiliate URL" value={result.affiliateUrl} />
        <ResultField label="Short URL" value={shareUrl} />

        <Button type="button" className="w-full" onClick={handleCopy}>
          {copied ? <Check /> : <Copy />}
          {copied ? "Copied" : "Copy link"}
        </Button>
      </CardContent>
    </Card>
  );
}

interface ResultFieldProps {
  label: string;
  value: string;
}

function ResultField({ label, value }: ResultFieldProps) {
  return (
    <div className="space-y-1.5">
      <p className="text-xs font-medium text-muted-foreground">{label}</p>
      <p className="break-all rounded-lg border bg-muted/30 px-3 py-2 text-sm">
        {value}
      </p>
    </div>
  );
}
