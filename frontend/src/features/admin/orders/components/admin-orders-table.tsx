"use client";

import { Eye, Package } from "lucide-react";
import { useEffect, useState } from "react";

import { EmptyState } from "@/components/empty-state";
import { OrderStatusBadge } from "@/components/status-badge";
import { TableFetchOverlay } from "@/components/table-fetch-overlay";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { AdminOrderDetailModal } from "@/features/admin/orders/components/admin-order-detail-modal";
import { AdminOrderFilters } from "@/features/admin/orders/components/admin-order-filters";
import { useAdminOrders } from "@/features/admin/orders/hooks/use-admin-orders";
import { OrdersPagination } from "@/features/orders/components/orders-pagination";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";
import type { AdminOrderListItem } from "@/types/admin-order";
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

  const hasActiveFilters =
    Boolean(orderIdSearch || userSearch || fromDate || toDate) ||
    statusFilter !== "all";
  const isEmpty = !isLoading && !isError && data?.items.length === 0;

  const handleClearFilters = () => {
    setStatusFilter("all");
    setOrderIdInput("");
    setUserInput("");
    setOrderIdSearch("");
    setUserSearch("");
    setFromDate("");
    setToDate("");
    setPage(1);
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

          <AdminOrderFilters
            orderId={orderIdInput}
            user={userInput}
            fromDate={fromDate}
            toDate={toDate}
            statusFilter={statusFilter}
            onOrderIdChange={setOrderIdInput}
            onUserChange={setUserInput}
            onFromDateChange={setFromDate}
            onToDateChange={setToDate}
            onStatusFilterChange={(value) => {
              setStatusFilter(value);
              setPage(1);
            }}
            onClearFilters={handleClearFilters}
            disabled={isLoading}
          />
        </CardHeader>

        <CardContent className="space-y-4">
          {isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(error)}
            </p>
          ) : null}

          {isLoading ? <AdminOrdersTableSkeleton /> : null}

          {isEmpty && !hasActiveFilters ? (
            <EmptyState
              icon={Package}
              title="No orders yet"
              description="Affiliate conversion orders will appear here."
            />
          ) : null}

          {isEmpty && hasActiveFilters ? (
            <EmptyState
              icon={Package}
              title="No matching orders"
              description="No orders found matching your filters."
              action={
                <Button type="button" variant="ghost" size="sm" onClick={handleClearFilters}>
                  Clear filters
                </Button>
              }
            />
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <TableFetchOverlay isFetching={isFetching}>
              <AdminOrdersDesktopTable
                orders={data.items}
                onViewDetail={(id) => {
                  setSelectedOrderId(id);
                  setIsDetailOpen(true);
                }}
              />
              <AdminOrdersMobileList
                orders={data.items}
                onViewDetail={(id) => {
                  setSelectedOrderId(id);
                  setIsDetailOpen(true);
                }}
              />
            </TableFetchOverlay>
          ) : null}
        </CardContent>

        {!isLoading && !isError && data && data.totalCount > 0 ? (
          <CardFooter className="border-t">
            <OrdersPagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
              disabled={isFetching}
            />
          </CardFooter>
        ) : null}
      </Card>

      <AdminOrderDetailModal
        orderId={selectedOrderId}
        open={isDetailOpen}
        onOpenChange={(open) => {
          setIsDetailOpen(open);
          if (!open) setSelectedOrderId(null);
        }}
      />
    </>
  );
}

function AdminOrdersDesktopTable({
  orders,
  onViewDetail,
}: {
  orders: AdminOrderListItem[];
  onViewDetail: (id: string) => void;
}) {
  return (
    <div className="hidden overflow-hidden rounded-xl border border-border md:block">
      <table className="w-full text-left text-sm">
        <thead className="sticky top-0 z-10 border-b border-border bg-muted/40">
          <tr>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Order ID</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">User</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Commission</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Cashback</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Status</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Created</th>
            <th scope="col" className="px-4 py-3 text-right text-xs font-medium text-muted-foreground">Actions</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id} className="border-b border-border last:border-b-0 hover:bg-muted/30">
              <td className="px-4 py-3 font-mono text-sm font-medium">{order.orderCode}</td>
              <td className="px-4 py-3">
                <p className="font-medium">{order.user.fullName}</p>
                <p className="text-xs text-muted-foreground">{order.user.email}</p>
              </td>
              <td className="px-4 py-3 tabular-nums">{formatCurrency(order.commissionAmount)}</td>
              <td className="px-4 py-3 font-medium tabular-nums">{formatCurrency(order.cashbackAmount)}</td>
              <td className="px-4 py-3">
                <OrderStatusBadge status={order.status} label={order.statusName} />
              </td>
              <td className="px-4 py-3 text-xs text-muted-foreground">{formatDateTime(order.createdAt)}</td>
              <td className="px-4 py-3 text-right">
                <Button type="button" variant="outline" size="sm" onClick={() => onViewDetail(order.id)}>
                  <Eye className="size-4" />
                  View
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function AdminOrdersMobileList({
  orders,
  onViewDetail,
}: {
  orders: AdminOrderListItem[];
  onViewDetail: (id: string) => void;
}) {
  return (
    <div className="space-y-3 md:hidden">
      {orders.map((order) => (
        <article key={order.id} className="space-y-3 rounded-xl border border-border p-4">
          <div className="flex items-start justify-between gap-3">
            <p className="font-mono text-sm font-medium">{order.orderCode}</p>
            <OrderStatusBadge status={order.status} label={order.statusName} className="shrink-0" />
          </div>
          <p className="text-sm font-medium">{order.user.fullName}</p>
          <p className="text-xs text-muted-foreground">{order.user.email}</p>
          <dl className="grid grid-cols-2 gap-3">
            <div>
              <dt className="text-xs text-muted-foreground">Commission</dt>
              <dd className="text-sm tabular-nums">{formatCurrency(order.commissionAmount)}</dd>
            </div>
            <div>
              <dt className="text-xs text-muted-foreground">Cashback</dt>
              <dd className="text-sm font-medium tabular-nums">{formatCurrency(order.cashbackAmount)}</dd>
            </div>
          </dl>
          <div className="flex items-center justify-between">
            <p className="text-xs text-muted-foreground">{formatDateTime(order.createdAt)}</p>
            <Button type="button" variant="outline" size="sm" onClick={() => onViewDetail(order.id)}>
              View
            </Button>
          </div>
        </article>
      ))}
    </div>
  );
}

function AdminOrdersTableSkeleton() {
  return (
    <>
      <div className="hidden overflow-hidden rounded-xl border border-border md:block">
        <div className="border-b border-border bg-muted/40 px-4 py-3">
          <Skeleton className="h-3 w-full max-w-lg" />
        </div>
        {Array.from({ length: 5 }).map((_, index) => (
          <div key={index} className="flex gap-4 border-b border-border px-4 py-3 last:border-b-0">
            <Skeleton className="h-4 w-24" />
            <Skeleton className="h-4 w-32" />
            <Skeleton className="h-4 w-20" />
            <Skeleton className="h-4 w-20" />
            <Skeleton className="h-4 w-16" />
            <Skeleton className="h-4 w-28" />
            <Skeleton className="ml-auto h-8 w-16" />
          </div>
        ))}
      </div>
      <div className="space-y-3 md:hidden">
        {Array.from({ length: 3 }).map((_, index) => (
          <Skeleton key={index} className="h-36 w-full rounded-xl" />
        ))}
      </div>
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
