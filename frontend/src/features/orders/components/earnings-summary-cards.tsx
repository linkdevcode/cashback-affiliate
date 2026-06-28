"use client";

import { MetricCard, MetricCardSkeleton } from "@/components/metric-card";
import { Card, CardContent } from "@/components/ui/card";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import { useOrderSummary } from "@/features/orders/hooks/use-order-summary";
import { isApiError } from "@/services/api-client";

const metricDefinitions = [
  {
    key: "totalCashback" as const,
    label: "Total Cashback",
    description: "Across all orders",
    featured: false,
  },
  {
    key: "pendingCashback" as const,
    label: "Pending Cashback",
    description: "Awaiting approval",
    featured: false,
  },
  {
    key: "approvedCashback" as const,
    label: "Approved Cashback",
    description: "Confirmed earnings",
    featured: true,
  },
  {
    key: "rejectedCashback" as const,
    label: "Rejected Cashback",
    description: "From rejected orders",
    featured: false,
  },
];

export function EarningsSummaryCards() {
  const { data, isLoading, isError, error } = useOrderSummary();

  if (isLoading) {
    return (
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
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

  const summary = data ?? {
    totalCashback: 0,
    pendingCashback: 0,
    approvedCashback: 0,
    rejectedCashback: 0,
  };

  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
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

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load earnings summary.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load earnings summary.";
}
