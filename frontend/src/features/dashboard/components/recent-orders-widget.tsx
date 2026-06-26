"use client";

import Link from "next/link";
import { Loader2 } from "lucide-react";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  formatCurrency,
  formatDateTime,
  getOrderStatusBadgeClass,
} from "@/features/orders/lib/order-formatters";
import type { OrderListItem } from "@/types/order";

interface RecentOrdersWidgetProps {
  orders: OrderListItem[];
  isLoading?: boolean;
}

export function RecentOrdersWidget({
  orders,
  isLoading = false,
}: RecentOrdersWidgetProps) {
  return (
    <Card>
      <CardHeader className="flex flex-col gap-2 sm:flex-row sm:items-start sm:justify-between">
        <div>
          <CardTitle>Recent Orders</CardTitle>
          <CardDescription>Latest affiliate conversions and cashback</CardDescription>
        </div>
        <Link
          href="/dashboard/orders"
          className="text-sm font-medium text-primary hover:underline"
        >
          View all orders
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? (
          <div className="flex items-center justify-center py-12 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading recent orders...
          </div>
        ) : null}

        {!isLoading && orders.length === 0 ? (
          <p className="py-12 text-center text-sm text-muted-foreground">
            No recent orders yet.
          </p>
        ) : null}

        {!isLoading && orders.length > 0 ? (
          <div className="overflow-x-auto">
            <table className="w-full min-w-[36rem] text-left text-sm">
              <thead>
                <tr className="border-b text-muted-foreground">
                  <th className="px-2 py-3 font-medium">Order ID</th>
                  <th className="px-2 py-3 font-medium">Status</th>
                  <th className="px-2 py-3 font-medium">Cashback</th>
                  <th className="px-2 py-3 font-medium">Created</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.id} className="border-b last:border-0">
                    <td className="px-2 py-3 font-medium">{order.orderCode}</td>
                    <td className="px-2 py-3">
                      <span
                        className={`inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium ${getOrderStatusBadgeClass(order.status)}`}
                      >
                        {order.statusName}
                      </span>
                    </td>
                    <td className="px-2 py-3 font-medium">
                      {formatCurrency(order.cashbackAmount)}
                    </td>
                    <td className="px-2 py-3 text-muted-foreground">
                      {formatDateTime(order.createdAt)}
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
