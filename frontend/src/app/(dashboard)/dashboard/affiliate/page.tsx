import Link from "next/link";

import { LinkConverterForm } from "@/features/affiliate-links/components/link-converter-form";

export default function AffiliatePage() {
  return (
    <div className="mx-auto max-w-2xl space-y-6">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Link Converter</h1>
          <p className="mt-2 text-sm text-muted-foreground">
            Paste a Shopee product URL to create your cashback affiliate link.
          </p>
        </div>

        <Link
          href="/dashboard/affiliate/history"
          className="text-sm font-medium text-primary hover:underline"
        >
          View history
        </Link>
      </div>

      <LinkConverterForm />
    </div>
  );
}
