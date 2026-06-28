"use client";

import { useAdminDashboard } from "@/features/admin/dashboard/hooks/use-admin-dashboard";
import { AdminOrdersChart } from "@/features/admin/dashboard/components/admin-orders-chart";
import { AdminRecentOrdersWidget } from "@/features/admin/dashboard/components/admin-recent-orders-widget";
import { AdminRecentUsersWidget } from "@/features/admin/dashboard/components/admin-recent-users-widget";
import { AdminRecentWithdrawalsWidget } from "@/features/admin/dashboard/components/admin-recent-withdrawals-widget";
import { AdminRevenueChart } from "@/features/admin/dashboard/components/admin-revenue-chart";
import {
  AdminPendingActionsBanner,
  AdminStatisticsCards,
} from "@/features/admin/dashboard/components/admin-statistics-cards";
import { emptyAdminDashboardSummary } from "@/types/admin-dashboard";

export function AdminDashboardOverview() {
  const { data, isLoading, isError, error } = useAdminDashboard();
  const summary = data ?? emptyAdminDashboardSummary;

  return (
    <div className="space-y-6">
      {!isLoading ? (
        <AdminPendingActionsBanner pendingWithdrawals={summary.pendingWithdrawals} />
      ) : null}

      <AdminStatisticsCards
        summary={summary}
        isLoading={isLoading}
        isError={isError}
        error={error}
      />

      <div className="grid grid-cols-1 gap-6 xl:grid-cols-2">
        <AdminOrdersChart ordersByMonth={summary.ordersByMonth} isLoading={isLoading} />
        <AdminRevenueChart revenueByMonth={summary.revenueByMonth} isLoading={isLoading} />
      </div>

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-3">
        <AdminRecentUsersWidget users={summary.recentUsers} isLoading={isLoading} />
        <AdminRecentOrdersWidget orders={summary.recentOrders} isLoading={isLoading} />
        <AdminRecentWithdrawalsWidget
          withdrawals={summary.recentWithdrawals}
          isLoading={isLoading}
        />
      </div>
    </div>
  );
}
