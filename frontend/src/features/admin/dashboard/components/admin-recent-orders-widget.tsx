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
  getOrderStatusBadgeClass,
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
        <div>
          <CardTitle>Recent orders</CardTitle>
          <CardDescription>Latest affiliate conversions</CardDescription>
        </div>
        <Link
          href="/admin/orders"
          className="text-sm font-medium text-primary hover:underline"
        >
          View all
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? (
          <div className="flex items-center justify-center py-10 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading recent orders...
          </div>
        ) : null}

        {!isLoading && orders.length === 0 ? (
          <p className="py-10 text-center text-sm text-muted-foreground">
            No recent orders yet.
          </p>
        ) : null}

        {!isLoading && orders.length > 0 ? (
          <div className="overflow-x-auto">
            <table className="w-full min-w-[28rem] text-left text-sm">
              <thead>
                <tr className="border-b text-muted-foreground">
                  <th className="px-2 py-2 font-medium">Order ID</th>
                  <th className="px-2 py-2 font-medium">User</th>
                  <th className="px-2 py-2 font-medium">Status</th>
                  <th className="px-2 py-2 font-medium">Cashback</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.id} className="border-b last:border-0">
                    <td className="px-2 py-2 font-medium">{order.orderCode}</td>
                    <td className="px-2 py-2 text-muted-foreground">
                      {order.userEmail}
                    </td>
                    <td className="px-2 py-2">
                      <span
                        className={`inline-flex rounded-full px-2 py-0.5 text-xs font-medium ${getOrderStatusBadgeClass(order.status)}`}
                      >
                        {order.statusName}
                      </span>
                    </td>
                    <td className="px-2 py-2 font-medium">
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
