import type { OrderStatus } from "@/types/order";
import type { UserStatus } from "@/types/auth";
import type { WithdrawalStatus } from "@/types/withdrawal";

export interface MonthlyOrderCount {
  year: number;
  month: number;
  orderCount: number;
}

export interface MonthlyRevenue {
  year: number;
  month: number;
  revenueAmount: number;
}

export interface AdminRecentUser {
  id: string;
  email: string;
  fullName: string;
  status: UserStatus;
  statusName: string;
  createdAt: string;
}

export interface AdminRecentOrder {
  id: string;
  orderCode: string;
  userEmail: string;
  status: OrderStatus;
  statusName: string;
  cashbackAmount: number;
  createdAt: string;
}

export interface AdminRecentWithdrawal {
  id: string;
  userEmail: string;
  amount: number;
  status: WithdrawalStatus;
  statusName: string;
  requestedAt: string;
}

export interface AdminDashboardSummary {
  totalUsers: number;
  activeUsers: number;
  suspendedUsers: number;
  totalOrders: number;
  pendingOrders: number;
  approvedOrders: number;
  rejectedOrders: number;
  totalWithdrawals: number;
  pendingWithdrawals: number;
  completedWithdrawals: number;
  totalCommission: number;
  totalCashbackPaid: number;
  platformRevenue: number;
  ordersByMonth: MonthlyOrderCount[];
  revenueByMonth: MonthlyRevenue[];
  recentUsers: AdminRecentUser[];
  recentOrders: AdminRecentOrder[];
  recentWithdrawals: AdminRecentWithdrawal[];
}

export const emptyAdminDashboardSummary: AdminDashboardSummary = {
  totalUsers: 0,
  activeUsers: 0,
  suspendedUsers: 0,
  totalOrders: 0,
  pendingOrders: 0,
  approvedOrders: 0,
  rejectedOrders: 0,
  totalWithdrawals: 0,
  pendingWithdrawals: 0,
  completedWithdrawals: 0,
  totalCommission: 0,
  totalCashbackPaid: 0,
  platformRevenue: 0,
  ordersByMonth: [],
  revenueByMonth: [],
  recentUsers: [],
  recentOrders: [],
  recentWithdrawals: [],
};
