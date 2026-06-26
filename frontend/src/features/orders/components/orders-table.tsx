"use client";

import { Eye, Loader2 } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { OrderDetailModal } from "@/features/orders/components/order-detail-modal";
import { OrderStatusFilter as OrderStatusFilterBar } from "@/features/orders/components/order-status-filter";
import { OrdersPagination } from "@/features/orders/components/orders-pagination";
import { useOrders } from "@/features/orders/hooks/use-orders";
import {
  formatCurrency,
  formatDateTime,
  getOrderStatusBadgeClass,
} from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";
import { OrderStatus, type OrderStatusFilter } from "@/types/order";

const PAGE_SIZE = 20;

export function OrdersTable() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState<OrderStatusFilter>("all");
  const [selectedOrderId, setSelectedOrderId] = useState<string | null>(null);
  const [isDetailOpen, setIsDetailOpen] = useState(false);

  const status = statusFilter === "all" ? undefined : statusFilter;

  const { data, isLoading, isError, error, isFetching } = useOrders({
    page,
    pageSize: PAGE_SIZE,
    status,
  });

  const handleStatusFilterChange = (value: OrderStatusFilter) => {
    setStatusFilter(value);
    setPage(1);
  };

  const handleViewDetail = (orderId: string) => {
    setSelectedOrderId(orderId);
    setIsDetailOpen(true);
  };

  const handleDetailOpenChange = (open: boolean) => {
    setIsDetailOpen(open);

    if (!open) {
      setSelectedOrderId(null);
    }
  };

  return (
    <>
      <Card>
        <CardHeader className="gap-4">
          <div>
            <CardTitle>Orders</CardTitle>
            <CardDescription>
              Track your affiliate conversions, commission, and cashback.
            </CardDescription>
          </div>

          <OrderStatusFilterBar
            value={statusFilter}
            onChange={handleStatusFilterChange}
            disabled={isLoading || isFetching}
          />
        </CardHeader>

        <CardContent className="space-y-4">
          {isLoading ? (
            <div className="flex items-center justify-center py-12 text-sm text-muted-foreground">
              <Loader2 className="mr-2 size-4 animate-spin" />
              Loading orders...
            </div>
          ) : null}

          {isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(error)}
            </p>
          ) : null}

          {!isLoading && !isError && data?.items.length === 0 ? (
            <p className="py-12 text-center text-sm text-muted-foreground">
              {statusFilter === "all"
                ? "No orders yet. Generate an affiliate link and make a purchase to see orders here."
                : `No ${getFilterEmptyLabel(statusFilter)} orders found.`}
            </p>
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <div className="overflow-x-auto rounded-lg border">
              <table className="w-full min-w-[720px] text-left text-sm">
                <thead className="border-b bg-muted/40">
                  <tr>
                    <th className="px-4 py-3 font-medium">Order ID</th>
                    <th className="px-4 py-3 font-medium">Status</th>
                    <th className="px-4 py-3 font-medium">Commission</th>
                    <th className="px-4 py-3 font-medium">Cashback</th>
                    <th className="px-4 py-3 font-medium">Created at</th>
                    <th className="px-4 py-3 font-medium text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {data.items.map((order) => (
                    <tr key={order.id} className="border-b last:border-b-0">
                      <td className="px-4 py-3 font-medium">{order.orderCode}</td>
                      <td className="px-4 py-3">
                        <span
                          className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getOrderStatusBadgeClass(order.status)}`}
                        >
                          {order.statusName}
                        </span>
                      </td>
                      <td className="px-4 py-3">{formatCurrency(order.commissionAmount)}</td>
                      <td className="px-4 py-3">{formatCurrency(order.cashbackAmount)}</td>
                      <td className="whitespace-nowrap px-4 py-3 text-muted-foreground">
                        {formatDateTime(order.createdAt)}
                      </td>
                      <td className="px-4 py-3 text-right">
                        <Button
                          type="button"
                          variant="outline"
                          size="sm"
                          onClick={() => handleViewDetail(order.id)}
                        >
                          <Eye />
                          View
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : null}

          {!isLoading && !isError && data && data.totalCount > 0 ? (
            <OrdersPagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
              disabled={isFetching}
            />
          ) : null}
        </CardContent>
      </Card>

      <OrderDetailModal
        orderId={selectedOrderId}
        open={isDetailOpen}
        onOpenChange={handleDetailOpenChange}
      />
    </>
  );
}

function getFilterEmptyLabel(status: OrderStatusFilter): string {
  switch (status) {
    case OrderStatus.Pending:
      return "pending";
    case OrderStatus.Approved:
      return "approved";
    case OrderStatus.Rejected:
      return "rejected";
    default:
      return "";
  }
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load orders.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load orders.";
}
