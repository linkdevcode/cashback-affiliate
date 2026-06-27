import Link from "next/link";
import { Wallet } from "lucide-react";

import { EmptyState } from "@/components/empty-state";
import { MetricCard, MetricCardSkeleton } from "@/components/metric-card";
import { buttonVariants } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";
import { emptyDashboardSummary, type DashboardSummary } from "@/types/dashboard";
import { cn } from "@/lib/utils";

interface DashboardSummaryCardsProps {
  summary?: DashboardSummary;
  isLoading?: boolean;
  isError?: boolean;
  error?: unknown;
}

const metricDefinitions = [
  {
    key: "availableBalance" as const,
    label: "Available Balance",
    description: "Ready to withdraw",
    featured: true,
  },
  {
    key: "pendingCashback" as const,
    label: "Pending Cashback",
    description: "Awaiting approval",
    featured: false,
  },
  {
    key: "totalCashback" as const,
    label: "Total Cashback",
    description: "Approved and pending",
    featured: false,
  },
  {
    key: "totalWithdrawn" as const,
    label: "Total Withdrawn",
    description: "Completed payouts",
    featured: false,
  },
];

export function DashboardSummaryCards({
  summary = emptyDashboardSummary,
  isLoading = false,
  isError = false,
  error,
}: DashboardSummaryCardsProps) {
  if (isLoading) {
    return (
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
        {metricDefinitions.map((metric) => (
          <MetricCardSkeleton key={metric.key} featured={metric.featured} />
        ))}
      </div>
    );
  }

  if (isError) {
    return (
      <Card>
        <CardContent className="py-6">
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
      {metricDefinitions.map((metric) => (
        <MetricCard
          key={metric.key}
          label={metric.label}
          value={formatCurrency(summary[metric.key])}
          description={metric.description}
          featured={metric.featured}
        />
      ))}
    </div>
  );
}

interface DashboardEmptyBannerProps {
  summary: DashboardSummary;
}

export function DashboardEmptyBanner({ summary }: DashboardEmptyBannerProps) {
  if (!isDashboardEmpty(summary)) {
    return null;
  }

  return (
    <Card>
      <CardContent>
        <EmptyState
          icon={Wallet}
          title="No earnings yet"
          description="Generate an affiliate link and make a purchase to start earning cashback."
          action={
            <Link
              href="/dashboard/affiliate"
              className={cn(buttonVariants({ variant: "outline" }))}
            >
              Create affiliate link
            </Link>
          }
        />
      </CardContent>
    </Card>
  );
}

export function isDashboardEmpty(summary: DashboardSummary): boolean {
  const hasChartData = summary.cashbackByMonth.some(
    (item) => item.cashbackAmount > 0,
  );

  return (
    summary.availableBalance === 0 &&
    summary.pendingCashback === 0 &&
    summary.totalCashback === 0 &&
    summary.totalWithdrawn === 0 &&
    summary.recentOrders.length === 0 &&
    !hasChartData
  );
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load dashboard summary.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load dashboard summary.";
}
