"use client";

import Link from "next/link";
import { Package } from "lucide-react";

import { EmptyState } from "@/components/empty-state";
import { OrderStatusBadge } from "@/components/status-badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import {
  formatCurrency,
} from "@/features/orders/lib/order-formatters";
import type { AdminRecentOrder } from "@/types/admin-dashboard";

interface AdminRecentOrdersWidgetProps {
  orders: AdminRecentOrder[];
  isLoading?: boolean;
}

export function AdminRecentOrdersWidget({
  orders,
  isLoading = false,
}: AdminRecentOrdersWidgetProps) {
  return (
    <Card className="h-full">
      <CardHeader className="flex flex-col gap-2 sm:flex-row sm:items-start sm:justify-between">
        <div className="space-y-1">
          <CardTitle className="text-lg font-semibold">Recent orders</CardTitle>
          <CardDescription>Latest affiliate conversions</CardDescription>
        </div>
        <Link href="/admin/orders" className="text-sm font-medium text-brand hover:underline">
          View all
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? <RecentOrdersSkeleton /> : null}

        {!isLoading && orders.length === 0 ? (
          <EmptyState
            icon={Package}
            title="No recent orders"
            description="New affiliate conversions will appear here."
          />
        ) : null}

        {!isLoading && orders.length > 0 ? (
          <div className="overflow-hidden rounded-xl border border-border">
            <table className="w-full text-left text-sm">
              <thead className="border-b border-border bg-muted/40">
                <tr>
                  <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Order ID</th>
                  <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">User</th>
                  <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Status</th>
                  <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Cashback</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.id} className="border-b border-border last:border-b-0 hover:bg-muted/30">
                    <td className="px-4 py-3 font-mono text-sm font-medium">{order.orderCode}</td>
                    <td className="px-4 py-3 text-xs text-muted-foreground">{order.userEmail}</td>
                    <td className="px-4 py-3">
                      <OrderStatusBadge status={order.status} label={order.statusName} />
                    </td>
                    <td className="px-4 py-3 font-medium tabular-nums">
                      {formatCurrency(order.cashbackAmount)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : null}
      </CardContent>
    </Card>
  );
}

function RecentOrdersSkeleton() {
  return (
    <div className="space-y-3">
      {Array.from({ length: 4 }).map((_, index) => (
        <Skeleton key={index} className="h-10 w-full rounded-lg" />
      ))}
    </div>
  );
}
