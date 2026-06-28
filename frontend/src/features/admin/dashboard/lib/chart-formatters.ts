import { formatMonthLabel } from "@/features/dashboard/lib/chart-formatters";
import type { MonthlyOrderCount, MonthlyRevenue } from "@/types/admin-dashboard";

export function toOrdersChartData(ordersByMonth: MonthlyOrderCount[]) {
  return ordersByMonth.map((item) => ({
    month: formatMonthLabel(item.year, item.month),
    orders: item.orderCount,
  }));
}

export function toRevenueChartData(revenueByMonth: MonthlyRevenue[]) {
  return revenueByMonth.map((item) => ({
    month: formatMonthLabel(item.year, item.month),
    revenue: item.revenueAmount,
  }));
}
