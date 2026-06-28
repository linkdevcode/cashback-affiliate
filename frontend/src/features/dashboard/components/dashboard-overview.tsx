"use client";

import { useDashboardSummary } from "@/features/dashboard/hooks/use-dashboard-summary";
import {
  DashboardEmptyBanner,
  DashboardSummaryCards,
} from "@/features/dashboard/components/dashboard-summary-cards";
import { DashboardQuickActions } from "@/features/dashboard/components/dashboard-quick-actions";
import { EarningsChart } from "@/features/dashboard/components/earnings-chart";
import { RecentActivities } from "@/features/dashboard/components/recent-activities";
import { RecentOrdersWidget } from "@/features/dashboard/components/recent-orders-widget";
import { emptyDashboardSummary } from "@/types/dashboard";

export function DashboardOverview() {
  const { data, isLoading, isError, error } = useDashboardSummary();
  const summary = data ?? emptyDashboardSummary;

  return (
    <div className="space-y-6">
      <DashboardSummaryCards
        summary={summary}
        isLoading={isLoading}
        isError={isError}
        error={error}
      />

      {!isLoading && !isError ? <DashboardEmptyBanner summary={summary} /> : null}

      <DashboardQuickActions />

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-3">
        <div className="lg:col-span-2">
          <EarningsChart
            cashbackByMonth={summary.cashbackByMonth}
            isLoading={isLoading}
          />
        </div>
        <RecentActivities orders={summary.recentOrders} isLoading={isLoading} />
      </div>

      <RecentOrdersWidget orders={summary.recentOrders} isLoading={isLoading} />
    </div>
  );
}
