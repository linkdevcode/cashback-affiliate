"use client";

import { useEffect, useState } from "react";
import { Eye, Loader2 } from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { AdminOrderDateRangeFilter } from "@/features/admin/orders/components/admin-order-date-range-filter";
import { AdminOrderDetailModal } from "@/features/admin/orders/components/admin-order-detail-modal";
import { AdminOrderSearchFilters } from "@/features/admin/orders/components/admin-order-search-filters";
import { AdminOrderStatusFilter } from "@/features/admin/orders/components/admin-order-status-filter";
import { AdminOrdersPagination } from "@/features/admin/orders/components/admin-orders-pagination";
import { useAdminOrders } from "@/features/admin/orders/hooks/use-admin-orders";
import {
  formatCurrency,
  formatDateTime,
  getOrderStatusBadgeClass,
} from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";
import type { OrderStatusFilter } from "@/types/order";

const PAGE_SIZE = 20;
const SEARCH_DEBOUNCE_MS = 400;

export function AdminOrdersTable() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState<OrderStatusFilter>("all");
  const [orderIdInput, setOrderIdInput] = useState("");
  const [userInput, setUserInput] = useState("");
  const [orderIdSearch, setOrderIdSearch] = useState("");
  const [userSearch, setUserSearch] = useState("");
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");
  const [selectedOrderId, setSelectedOrderId] = useState<string | null>(null);
  const [isDetailOpen, setIsDetailOpen] = useState(false);

  useEffect(() => {
    const timer = window.setTimeout(() => {
      setOrderIdSearch(orderIdInput.trim());
      setUserSearch(userInput.trim());
      setPage(1);
    }, SEARCH_DEBOUNCE_MS);

    return () => window.clearTimeout(timer);
  }, [orderIdInput, userInput]);

  const status = statusFilter === "all" ? undefined : statusFilter;

  const { data, isLoading, isError, error, isFetching } = useAdminOrders({
    page,
    pageSize: PAGE_SIZE,
    orderId: orderIdSearch || undefined,
    user: userSearch || undefined,
    status,
    fromDate: fromDate || undefined,
    toDate: toDate || undefined,
  });

  const handleStatusFilterChange = (value: OrderStatusFilter) => {
    setStatusFilter(value);
    setPage(1);
  };

  const handleFromDateChange = (value: string) => {
    setFromDate(value);
    setPage(1);
  };

  const handleToDateChange = (value: string) => {
    setToDate(value);
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
              Review platform orders, filter by status or date, and inspect user conversions.
            </CardDescription>
          </div>

          <AdminOrderSearchFilters
            orderId={orderIdInput}
            user={userInput}
            onOrderIdChange={setOrderIdInput}
            onUserChange={setUserInput}
            disabled={isLoading || isFetching}
          />

          <AdminOrderDateRangeFilter
            fromDate={fromDate}
            toDate={toDate}
            onFromDateChange={handleFromDateChange}
            onToDateChange={handleToDateChange}
            disabled={isLoading || isFetching}
          />

          <AdminOrderStatusFilter
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
              No orders found matching your filters.
            </p>
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <div className="overflow-x-auto rounded-lg border">
              <table className="w-full min-w-[960px] text-left text-sm">
                <thead className="border-b bg-muted/40">
                  <tr>
                    <th className="px-4 py-3 font-medium">Order ID</th>
                    <th className="px-4 py-3 font-medium">User</th>
                    <th className="px-4 py-3 font-medium">Commission</th>
                    <th className="px-4 py-3 font-medium">Cashback</th>
                    <th className="px-4 py-3 font-medium">Status</th>
                    <th className="px-4 py-3 font-medium">Created date</th>
                    <th className="px-4 py-3 font-medium text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {data.items.map((order) => (
                    <tr key={order.id} className="border-b last:border-b-0">
                      <td className="px-4 py-3 font-medium">{order.orderCode}</td>
                      <td className="px-4 py-3">
                        <p className="font-medium">{order.user.fullName}</p>
                        <p className="text-xs text-muted-foreground">{order.user.email}</p>
                      </td>
                      <td className="px-4 py-3">{formatCurrency(order.commissionAmount)}</td>
                      <td className="px-4 py-3">{formatCurrency(order.cashbackAmount)}</td>
                      <td className="px-4 py-3">
                        <span
                          className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getOrderStatusBadgeClass(order.status)}`}
                        >
                          {order.statusName}
                        </span>
                      </td>
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
            <AdminOrdersPagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
              disabled={isFetching}
            />
          ) : null}
        </CardContent>
      </Card>

      <AdminOrderDetailModal
        orderId={selectedOrderId}
        open={isDetailOpen}
        onOpenChange={handleDetailOpenChange}
      />
    </>
  );
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
