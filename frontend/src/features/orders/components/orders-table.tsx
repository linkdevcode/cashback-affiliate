"use client";

import { Eye, Package } from "lucide-react";
import Link from "next/link";
import { useMemo, useState } from "react";

import { EmptyState } from "@/components/empty-state";
import { OrderStatusBadge } from "@/components/status-badge";
import { Button, buttonVariants } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { OrderDetailModal } from "@/features/orders/components/order-detail-modal";
import {
  hasOrderFiltersActive,
  OrderFilters,
} from "@/features/orders/components/order-filters";
import { OrdersPagination } from "@/features/orders/components/orders-pagination";
import { useOrders } from "@/features/orders/hooks/use-orders";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import { cn } from "@/lib/utils";
import { isApiError } from "@/services/api-client";
import { OrderStatus, type OrderListItem, type OrderStatusFilter } from "@/types/order";

const PAGE_SIZE = 20;

export function OrdersTable() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState<OrderStatusFilter>("all");
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedOrderId, setSelectedOrderId] = useState<string | null>(null);
  const [isDetailOpen, setIsDetailOpen] = useState(false);

  const status = statusFilter === "all" ? undefined : statusFilter;

  const { data, isLoading, isError, error, isFetching } = useOrders({
    page,
    pageSize: PAGE_SIZE,
    status,
  });

  const filteredItems = useMemo(() => {
    if (!data?.items) {
      return [];
    }

    const query = searchQuery.trim().toLowerCase();
    if (!query) {
      return data.items;
    }

    return data.items.filter((order) =>
      order.orderCode.toLowerCase().includes(query),
    );
  }, [data?.items, searchQuery]);

  const hasActiveFilters = hasOrderFiltersActive(statusFilter, searchQuery);
  const isFilteredEmpty = !isLoading && !isError && data && filteredItems.length === 0;
  const isUnfilteredEmpty = !isLoading && !isError && data?.items.length === 0;

  const handleStatusFilterChange = (value: OrderStatusFilter) => {
    setStatusFilter(value);
    setPage(1);
  };

  const handleSearchQueryChange = (value: string) => {
    setSearchQuery(value);
  };

  const handleClearFilters = () => {
    setStatusFilter("all");
    setSearchQuery("");
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
            <CardTitle>Order history</CardTitle>
            <CardDescription>
              Track your affiliate conversions, commission, and cashback.
            </CardDescription>
          </div>

          <OrderFilters
            statusFilter={statusFilter}
            searchQuery={searchQuery}
            onStatusFilterChange={handleStatusFilterChange}
            onSearchQueryChange={handleSearchQueryChange}
            onClearFilters={handleClearFilters}
            disabled={isLoading}
            hasActiveFilters={hasActiveFilters}
          />
        </CardHeader>

        <CardContent className="space-y-4">
          {isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(error)}
            </p>
          ) : null}

          {isLoading ? <OrdersTableSkeleton /> : null}

          {isUnfilteredEmpty && !hasActiveFilters ? (
            <EmptyState
              icon={Package}
              title="No orders yet"
              description="Generate an affiliate link and make a purchase to see orders here."
              action={
                <Link
                  href="/dashboard/affiliate"
                  className={buttonVariants({ variant: "outline", size: "sm" })}
                >
                  Generate link
                </Link>
              }
            />
          ) : null}

          {isFilteredEmpty && hasActiveFilters ? (
            <EmptyState
              icon={Package}
              title="No matching orders"
              description={
                searchQuery
                  ? "No orders on this page match your search. Try a different order ID or clear filters."
                  : `No ${getFilterEmptyLabel(statusFilter)} orders found.`
              }
              action={
                <Button
                  type="button"
                  variant="ghost"
                  size="sm"
                  onClick={handleClearFilters}
                >
                  Clear filters
                </Button>
              }
            />
          ) : null}

          {!isLoading && !isError && filteredItems.length > 0 ? (
            <div className="relative">
              {isFetching ? (
                <div
                  className="absolute inset-x-0 top-0 z-10 h-0.5 overflow-hidden rounded-full bg-muted"
                  aria-hidden="true"
                >
                  <div className="h-full w-1/3 animate-pulse bg-brand" />
                </div>
              ) : null}

              <div
                className={cn(
                  "transition-opacity",
                  isFetching && "pointer-events-none opacity-60",
                )}
              >
                <OrdersDesktopTable
                  orders={filteredItems}
                  onViewDetail={handleViewDetail}
                />
                <OrdersMobileList
                  orders={filteredItems}
                  onViewDetail={handleViewDetail}
                />
              </div>

              {isFetching ? (
                <p className="mt-2 text-xs text-muted-foreground">Updating…</p>
              ) : null}
            </div>
          ) : null}
        </CardContent>

        {!isLoading && !isError && data && data.totalCount > 0 && !searchQuery ? (
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

      <OrderDetailModal
        orderId={selectedOrderId}
        open={isDetailOpen}
        onOpenChange={handleDetailOpenChange}
      />
    </>
  );
}

interface OrdersListProps {
  orders: OrderListItem[];
  onViewDetail: (orderId: string) => void;
}

function OrdersDesktopTable({ orders, onViewDetail }: OrdersListProps) {
  return (
    <div className="hidden overflow-hidden rounded-xl border border-border md:block">
      <table className="w-full text-left text-sm">
        <thead className="sticky top-0 z-10 border-b border-border bg-muted/40">
          <tr>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Order ID
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Status
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Commission
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Cashback
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Created
            </th>
            <th scope="col" className="px-4 py-3 text-right text-xs font-medium text-muted-foreground">
              Actions
            </th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr
              key={order.id}
              className="border-b border-border last:border-b-0 hover:bg-muted/30"
            >
              <td className="px-4 py-3 font-medium font-mono text-sm">{order.orderCode}</td>
              <td className="px-4 py-3">
                <OrderStatusBadge status={order.status} label={order.statusName} />
              </td>
              <td className="px-4 py-3 tabular-nums">{formatCurrency(order.commissionAmount)}</td>
              <td className="px-4 py-3 font-medium tabular-nums">
                {formatCurrency(order.cashbackAmount)}
              </td>
              <td className="px-4 py-3 text-xs text-muted-foreground">
                {formatDateTime(order.createdAt)}
              </td>
              <td className="px-4 py-3 text-right">
                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  onClick={() => onViewDetail(order.id)}
                >
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

function OrdersMobileList({ orders, onViewDetail }: OrdersListProps) {
  return (
    <div className="space-y-3 md:hidden">
      {orders.map((order) => (
        <article
          key={order.id}
          className="space-y-3 rounded-xl border border-border p-4"
        >
          <div className="flex items-start justify-between gap-3">
            <h3 className="font-mono text-sm font-medium">{order.orderCode}</h3>
            <OrderStatusBadge
              status={order.status}
              label={order.statusName}
              className="shrink-0"
            />
          </div>

          <dl className="grid grid-cols-2 gap-3">
            <div>
              <dt className="text-xs text-muted-foreground">Commission</dt>
              <dd className="text-sm tabular-nums">{formatCurrency(order.commissionAmount)}</dd>
            </div>
            <div>
              <dt className="text-xs text-muted-foreground">Cashback</dt>
              <dd className="text-sm font-medium tabular-nums">
                {formatCurrency(order.cashbackAmount)}
              </dd>
            </div>
            <div className="col-span-2">
              <dt className="text-xs text-muted-foreground">Created</dt>
              <dd className="text-xs text-muted-foreground">
                {formatDateTime(order.createdAt)}
              </dd>
            </div>
          </dl>

          <div className="flex justify-end">
            <Button
              type="button"
              variant="outline"
              size="sm"
              onClick={() => onViewDetail(order.id)}
            >
              View
            </Button>
          </div>
        </article>
      ))}
    </div>
  );
}

function OrdersTableSkeleton() {
  return (
    <>
      <div className="hidden overflow-hidden rounded-xl border border-border md:block">
        <div className="border-b border-border bg-muted/40 px-4 py-3">
          <Skeleton className="h-3 w-full max-w-lg" />
        </div>
        {Array.from({ length: 6 }).map((_, index) => (
          <div
            key={index}
            className="flex gap-4 border-b border-border px-4 py-3 last:border-b-0"
          >
            <Skeleton className="h-4 w-24" />
            <Skeleton className="h-4 w-16" />
            <Skeleton className="h-4 w-20" />
            <Skeleton className="h-4 w-20" />
            <Skeleton className="h-4 w-28" />
            <Skeleton className="ml-auto h-8 w-16" />
          </div>
        ))}
      </div>
      <div className="space-y-3 md:hidden">
        {Array.from({ length: 4 }).map((_, index) => (
          <Skeleton key={index} className="h-36 w-full rounded-xl" />
        ))}
      </div>
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
