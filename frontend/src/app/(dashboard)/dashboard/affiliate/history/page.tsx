import Link from "next/link";

import { AffiliateLinkHistoryTable } from "@/features/affiliate-links/components/affiliate-link-history-table";

export default function AffiliateHistoryPage() {
  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Link History</h1>
          <p className="mt-2 text-sm text-muted-foreground">
            Browse your generated affiliate links and view full details.
          </p>
        </div>

        <Link
          href="/dashboard/affiliate"
          className="text-sm font-medium text-primary hover:underline"
        >
          Generate new link
        </Link>
      </div>

      <AffiliateLinkHistoryTable />
    </div>
  );
}
