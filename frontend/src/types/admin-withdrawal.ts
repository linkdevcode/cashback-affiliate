import type { WithdrawalStatus } from "@/types/withdrawal";

export interface AdminWithdrawalUser {
  id: string;
  email: string;
  fullName: string;
}

export interface AdminWithdrawalListItem {
  id: string;
  user: AdminWithdrawalUser;
  amount: number;
  status: WithdrawalStatus;
  statusName: string;
  requestedAt: string;
}

export interface AdminWithdrawalDetail {
  id: string;
  user: AdminWithdrawalUser;
  amount: number;
  bankName: string;
  bankAccountNumber: string;
  bankAccountHolder: string;
  status: WithdrawalStatus;
  statusName: string;
  requestedAt: string;
  processedAt: string | null;
}

export interface AdminWithdrawalsListResponse {
  items: AdminWithdrawalListItem[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface AdminWithdrawalsQueryParams {
  page?: number;
  pageSize?: number;
  user?: string;
  status?: WithdrawalStatus;
}

export interface AdminWithdrawalActionResult {
  id: string;
  status: number;
  statusName: string;
}

export interface RejectWithdrawalRequest {
  reason?: string;
}
