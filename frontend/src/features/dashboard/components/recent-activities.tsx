"use client";

import { Coins, Loader2, Package, RefreshCw } from "lucide-react";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  mapOrdersToActivities,
  type DashboardActivityType,
} from "@/features/dashboard/lib/activity-mapper";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import type { OrderListItem } from "@/types/order";

interface RecentActivitiesProps {
  orders: OrderListItem[];
  isLoading?: boolean;
}

export function RecentActivities({ orders, isLoading = false }: RecentActivitiesProps) {
  const activities = mapOrdersToActivities(orders);

  return (
    <Card className="h-full">
      <CardHeader>
        <CardTitle>Recent Activities</CardTitle>
        <CardDescription>
          Recent orders, status changes, and cashback updates
        </CardDescription>
      </CardHeader>
      <CardContent>
        {isLoading ? (
          <div className="flex items-center justify-center py-12 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading activities...
          </div>
        ) : null}

        {!isLoading && activities.length === 0 ? (
          <p className="py-12 text-center text-sm text-muted-foreground">
            No recent activity yet.
          </p>
        ) : null}

        {!isLoading && activities.length > 0 ? (
          <ul className="space-y-4">
            {activities.map((activity) => (
              <li key={activity.id} className="flex gap-3">
                <div className="mt-0.5 flex size-8 shrink-0 items-center justify-center rounded-full bg-muted">
                  <ActivityIcon type={activity.type} />
                </div>
                <div className="min-w-0 flex-1 space-y-1">
                  <div className="flex items-start justify-between gap-2">
                    <p className="text-sm font-medium">{activity.title}</p>
                    <time
                      className="shrink-0 text-xs text-muted-foreground"
                      dateTime={activity.timestamp}
                    >
                      {formatDateTime(activity.timestamp)}
                    </time>
                  </div>
                  <p className="text-sm text-muted-foreground">
                    {activity.description}
                  </p>
                  {activity.amount !== undefined ? (
                    <p className="text-sm font-medium text-emerald-600 dark:text-emerald-400">
                      {formatCurrency(activity.amount)}
                    </p>
                  ) : null}
                </div>
              </li>
            ))}
          </ul>
        ) : null}
      </CardContent>
    </Card>
  );
}

function ActivityIcon({ type }: { type: DashboardActivityType }) {
  switch (type) {
    case "order":
      return <Package className="size-4 text-muted-foreground" />;
    case "status":
      return <RefreshCw className="size-4 text-muted-foreground" />;
    case "cashback":
      return <Coins className="size-4 text-muted-foreground" />;
    default:
      return <Package className="size-4 text-muted-foreground" />;
  }
}
