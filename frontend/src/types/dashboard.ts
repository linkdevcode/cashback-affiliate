import type { OrderListItem } from "@/types/order";

export interface MonthlyCashback {
  year: number;
  month: number;
  cashbackAmount: number;
}

export interface DashboardSummary {
  availableBalance: number;
  pendingCashback: number;
  totalCashback: number;
  totalWithdrawn: number;
  recentOrders: OrderListItem[];
  cashbackByMonth: MonthlyCashback[];
}

export const emptyDashboardSummary: DashboardSummary = {
  availableBalance: 0,
  pendingCashback: 0,
  totalCashback: 0,
  totalWithdrawn: 0,
  recentOrders: [],
  cashbackByMonth: [],
};
