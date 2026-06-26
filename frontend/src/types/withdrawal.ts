export const WithdrawalStatus = {
  Pending: 1,
  Approved: 2,
  Rejected: 3,
  Completed: 4,
} as const;

export type WithdrawalStatus = (typeof WithdrawalStatus)[keyof typeof WithdrawalStatus];

export type WithdrawalStatusFilter = WithdrawalStatus | "all";

export const MIN_WITHDRAWAL_AMOUNT = 50_000;

export interface WithdrawalListItem {
  id: string;
  amount: number;
  status: WithdrawalStatus;
  statusName: string;
  requestedAt: string;
  processedAt: string | null;
}

export interface WithdrawalsListResponse {
  items: WithdrawalListItem[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface WithdrawalsQueryParams {
  page?: number;
  pageSize?: number;
  status?: WithdrawalStatus;
  sortBy?: string;
  direction?: "asc" | "desc";
}

export interface CreateWithdrawalRequest {
  amount: number;
  bankName: string;
  bankAccountNumber: string;
  bankAccountName: string;
}

export interface CreateWithdrawalResult {
  id: string;
  amount: number;
  status: WithdrawalStatus;
  requestedAt: string;
}
