export const OrderStatus = {
  Pending: 1,
  Approved: 2,
  Rejected: 3,
} as const;

export type OrderStatus = (typeof OrderStatus)[keyof typeof OrderStatus];

export type OrderStatusFilter = OrderStatus | "all";

export interface OrderListItem {
  id: string;
  orderCode: string;
  merchant: string | null;
  orderAmount: number | null;
  commissionAmount: number;
  cashbackAmount: number;
  status: OrderStatus;
  statusName: string;
  createdAt: string;
}

export interface OrderDetail {
  id: string;
  orderCode: string;
  merchant: string | null;
  orderAmount: number | null;
  commissionAmount: number;
  cashbackAmount: number;
  platformProfit: number;
  status: OrderStatus;
  statusName: string;
  createdAt: string;
}

export interface OrdersListResponse {
  items: OrderListItem[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface OrdersQueryParams {
  page?: number;
  pageSize?: number;
  status?: OrderStatus;
  sortBy?: string;
  direction?: "asc" | "desc";
}

export interface OrderSummary {
  totalOrders: number;
  pendingOrders: number;
  approvedOrders: number;
  rejectedOrders: number;
  totalCommission: number;
  totalCashback: number;
  pendingCashback: number;
  approvedCashback: number;
  rejectedCashback: number;
}
