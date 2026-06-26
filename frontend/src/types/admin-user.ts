import type { UserRole, UserStatus } from "@/types/auth";

export interface AdminUser {
  id: string;
  email: string;
  fullName: string;
  role: UserRole;
  status: UserStatus;
  createdAt: string;
}

export interface AdminUsersListResponse {
  items: AdminUser[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface AdminUserProfile {
  id: string;
  email: string;
  fullName: string;
  avatarUrl: string | null;
  role: UserRole;
  status: UserStatus;
  availableBalance: number;
  pendingBalance: number;
  lifetimeCashback: number;
  lastLoginAt: string | null;
  createdAt: string;
}

export interface AdminUserOrderSummary {
  totalOrders: number;
  pendingOrders: number;
  approvedOrders: number;
  rejectedOrders: number;
  totalCommission: number;
  totalCashback: number;
}

export interface AdminUserWithdrawalSummary {
  totalWithdrawals: number;
  pendingWithdrawals: number;
  approvedWithdrawals: number;
  rejectedWithdrawals: number;
  completedWithdrawals: number;
  totalWithdrawn: number;
}

export interface AdminUserDetail {
  profile: AdminUserProfile;
  orderSummary: AdminUserOrderSummary;
  withdrawalSummary: AdminUserWithdrawalSummary;
}

export interface AdminUserActionResult {
  id: string;
  status: UserStatus;
  statusName: string;
}

export interface AdminUsersQueryParams {
  page?: number;
  pageSize?: number;
  email?: string;
  name?: string;
  status?: UserStatus;
}

export type AdminUserStatusFilter = "all" | UserStatus;
