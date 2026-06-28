import type { OrderStatus } from "@/types/order";

export interface AdminOrderUser {
  id: string;
  email: string;
  fullName: string;
}

export interface AdminOrderListItem {
  id: string;
  orderCode: string;
  user: AdminOrderUser;
  commissionAmount: number;
  cashbackAmount: number;
  status: OrderStatus;
  statusName: string;
  createdAt: string;
}

export interface AdminOrderDetail {
  id: string;
  orderCode: string;
  user: AdminOrderUser;
  merchant: string | null;
  orderAmount: number | null;
  commissionAmount: number;
  cashbackAmount: number;
  platformProfit: number;
  status: OrderStatus;
  statusName: string;
  createdAt: string;
}

export interface AdminOrdersListResponse {
  items: AdminOrderListItem[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface AdminOrdersQueryParams {
  page?: number;
  pageSize?: number;
  orderId?: string;
  user?: string;
  status?: OrderStatus;
  fromDate?: string;
  toDate?: string;
}

export type AdminOrderStatusFilter = OrderStatus | "all";
