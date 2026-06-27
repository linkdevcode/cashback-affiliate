"use client";

import { Activity, Coins, Package, RefreshCw } from "lucide-react";

import { EmptyState } from "@/components/empty-state";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
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
        <CardTitle className="text-lg font-semibold">Recent activity</CardTitle>
        <CardDescription>Orders, status changes, and cashback updates</CardDescription>
      </CardHeader>
      <CardContent>
        {isLoading ? <RecentActivitiesSkeleton /> : null}

        {!isLoading && activities.length === 0 ? (
          <EmptyState
            icon={Activity}
            title="No activity yet"
            description="Your order and cashback updates will appear here."
          />
        ) : null}

        {!isLoading && activities.length > 0 ? (
          <ul className="space-y-4">
            {activities.map((activity) => (
              <li key={activity.id} className="flex gap-3">
                <div className="mt-0.5 shrink-0">
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
                  <p className="text-xs text-muted-foreground">
                    {activity.description}
                  </p>
                  {activity.amount !== undefined ? (
                    <p className="text-sm font-medium tabular-nums text-success">
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
  const iconClassName = "size-4 text-muted-foreground";

  switch (type) {
    case "order":
      return <Package className={iconClassName} aria-hidden="true" />;
    case "status":
      return <RefreshCw className={iconClassName} aria-hidden="true" />;
    case "cashback":
      return <Coins className={iconClassName} aria-hidden="true" />;
    default:
      return <Package className={iconClassName} aria-hidden="true" />;
  }
}

function RecentActivitiesSkeleton() {
  return (
    <ul className="space-y-4">
      {Array.from({ length: 4 }).map((_, index) => (
        <li key={index} className="flex gap-3">
          <Skeleton className="size-4 shrink-0 rounded-full" />
          <div className="flex-1 space-y-2">
            <div className="flex justify-between gap-2">
              <Skeleton className="h-4 w-32" />
              <Skeleton className="h-3 w-16" />
            </div>
            <Skeleton className="h-3 w-full" />
            <Skeleton className="h-4 w-20" />
          </div>
        </li>
      ))}
    </ul>
  );
}
