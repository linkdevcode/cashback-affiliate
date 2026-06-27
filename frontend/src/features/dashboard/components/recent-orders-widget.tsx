"use client";

import Link from "next/link";
import { Package } from "lucide-react";

import { EmptyState } from "@/components/empty-state";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { buttonVariants } from "@/components/ui/button";
import {
  formatCurrency,
  formatDateTime,
  getOrderStatusBadgeClass,
} from "@/features/orders/lib/order-formatters";
import type { OrderListItem } from "@/types/order";
import { cn } from "@/lib/utils";

const MAX_ROWS = 5;

interface RecentOrdersWidgetProps {
  orders: OrderListItem[];
  isLoading?: boolean;
}

export function RecentOrdersWidget({
  orders,
  isLoading = false,
}: RecentOrdersWidgetProps) {
  const visibleOrders = orders.slice(0, MAX_ROWS);

  return (
    <Card>
      <CardHeader className="flex flex-col gap-2 sm:flex-row sm:items-start sm:justify-between">
        <div className="space-y-1">
          <CardTitle className="text-lg font-semibold">Recent orders</CardTitle>
          <CardDescription>Latest affiliate conversions and cashback</CardDescription>
        </div>
        <Link
          href="/dashboard/orders"
          className="text-sm font-medium text-brand hover:underline"
        >
          View all orders
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? <RecentOrdersSkeleton /> : null}

        {!isLoading && visibleOrders.length === 0 ? (
          <EmptyState
            icon={Package}
            title="No orders yet"
            description="Generate an affiliate link and make a purchase to see orders here."
            action={
              <Link
                href="/dashboard/affiliate"
                className={cn(buttonVariants({ variant: "outline" }))}
              >
                Generate link
              </Link>
            }
          />
        ) : null}

        {!isLoading && visibleOrders.length > 0 ? (
          <>
            <div className="hidden overflow-hidden rounded-xl border border-border md:block">
              <table className="w-full text-left text-sm">
                <thead className="border-b border-border bg-muted/40">
                  <tr>
                    <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Order ID
                    </th>
                    <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Status
                    </th>
                    <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Cashback
                    </th>
                    <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Created
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {visibleOrders.map((order) => (
                    <tr
                      key={order.id}
                      className="border-b border-border last:border-b-0 hover:bg-muted/30"
                    >
                      <td className="px-4 py-3 font-medium">{order.orderCode}</td>
                      <td className="px-4 py-3">
                        <span
                          className={`inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium ${getOrderStatusBadgeClass(order.status)}`}
                        >
                          {order.statusName}
                        </span>
                      </td>
                      <td className="px-4 py-3 font-medium tabular-nums">
                        {formatCurrency(order.cashbackAmount)}
                      </td>
                      <td className="px-4 py-3 text-xs text-muted-foreground">
                        {formatDateTime(order.createdAt)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="space-y-3 md:hidden">
              {visibleOrders.map((order) => (
                <div
                  key={order.id}
                  className="space-y-3 rounded-xl border border-border p-4"
                >
                  <div className="flex items-start justify-between gap-3">
                    <p className="text-sm font-medium">{order.orderCode}</p>
                    <span
                      className={`inline-flex shrink-0 rounded-full px-2.5 py-0.5 text-xs font-medium ${getOrderStatusBadgeClass(order.status)}`}
                    >
                      {order.statusName}
                    </span>
                  </div>
                  <dl className="grid grid-cols-2 gap-3">
                    <div>
                      <dt className="text-xs text-muted-foreground">Cashback</dt>
                      <dd className="text-sm font-medium tabular-nums">
                        {formatCurrency(order.cashbackAmount)}
                      </dd>
                    </div>
                    <div>
                      <dt className="text-xs text-muted-foreground">Created</dt>
                      <dd className="text-xs text-muted-foreground">
                        {formatDateTime(order.createdAt)}
                      </dd>
                    </div>
                  </dl>
                </div>
              ))}
            </div>
          </>
        ) : null}
      </CardContent>
    </Card>
  );
}

function RecentOrdersSkeleton() {
  return (
    <>
      <div className="hidden space-y-0 overflow-hidden rounded-xl border border-border md:block">
        <div className="border-b border-border bg-muted/40 px-4 py-3">
          <Skeleton className="h-3 w-full max-w-md" />
        </div>
        {Array.from({ length: MAX_ROWS }).map((_, index) => (
          <div
            key={index}
            className="flex gap-4 border-b border-border px-4 py-3 last:border-b-0"
          >
            <Skeleton className="h-4 w-24" />
            <Skeleton className="h-4 w-16" />
            <Skeleton className="h-4 w-20" />
            <Skeleton className="h-4 w-28" />
          </div>
        ))}
      </div>
      <div className="space-y-3 md:hidden">
        {Array.from({ length: 3 }).map((_, index) => (
          <Skeleton key={index} className="h-28 w-full rounded-xl" />
        ))}
      </div>
    </>
  );
}
