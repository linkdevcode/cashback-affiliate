"use client";

import { useQueryClient } from "@tanstack/react-query";
import { Loader2 } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { AffiliateLinkResultCard } from "@/features/affiliate-links/components/affiliate-link-result-card";
import { affiliateLinksQueryKey } from "@/features/affiliate-links/hooks/use-affiliate-links";
import { useGenerateAffiliateLink } from "@/features/affiliate-links/hooks/use-generate-affiliate-link";
import { isApiError } from "@/services/api-client";

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    const apiError = error.response?.data;

    if (apiError?.errors?.length) {
      return apiError.errors.join(" ");
    }

    if (apiError?.message) {
      return apiError.message;
    }
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to generate affiliate link. Please try again.";
}

export function LinkConverterForm() {
  const [url, setUrl] = useState("");
  const queryClient = useQueryClient();
  const generateLink = useGenerateAffiliateLink();

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    generateLink.reset();

    const trimmedUrl = url.trim();

    if (!trimmedUrl) {
      return;
    }

    await generateLink.mutateAsync({ url: trimmedUrl });
    await queryClient.invalidateQueries({ queryKey: affiliateLinksQueryKey });
  };

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle>Paste Shopee product URL</CardTitle>
          <CardDescription>
            Enter a Shopee product link to generate your cashback tracking URL.
          </CardDescription>
        </CardHeader>

        <CardContent>
          <form className="space-y-4" onSubmit={handleSubmit}>
            <div className="space-y-2">
              <label htmlFor="product-url" className="text-sm font-medium">
                Product URL
              </label>
              <Input
                id="product-url"
                name="url"
                type="url"
                inputMode="url"
                autoComplete="off"
                placeholder="https://shopee.vn/product/..."
                value={url}
                onChange={(event) => setUrl(event.target.value)}
                disabled={generateLink.isPending}
                aria-invalid={generateLink.isError}
                required
              />
            </div>

            <Button
              type="submit"
              className="w-full"
              disabled={generateLink.isPending || !url.trim()}
            >
              {generateLink.isPending ? (
                <>
                  <Loader2 className="animate-spin" />
                  Generating...
                </>
              ) : (
                "Generate link"
              )}
            </Button>

            {generateLink.isError ? (
              <p className="text-sm text-destructive" role="alert">
                {getErrorMessage(generateLink.error)}
              </p>
            ) : null}
          </form>
        </CardContent>
      </Card>

      {generateLink.isSuccess && generateLink.data ? (
        <AffiliateLinkResultCard result={generateLink.data} />
      ) : null}
    </div>
  );
}
