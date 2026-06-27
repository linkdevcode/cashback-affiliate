"use client";

import { BarChart3 } from "lucide-react";
import { Bar, BarChart, CartesianGrid, Tooltip, XAxis, YAxis } from "recharts";

import { EmptyState } from "@/components/empty-state";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { ChartContainer } from "@/components/ui/chart";
import { Skeleton } from "@/components/ui/skeleton";
import { toOrdersChartData } from "@/features/admin/dashboard/lib/chart-formatters";
import type { MonthlyOrderCount } from "@/types/admin-dashboard";

const chartConfig = {
  orders: {
    label: "Orders",
    color: "var(--chart-orders)",
  },
};

interface AdminOrdersChartProps {
  ordersByMonth: MonthlyOrderCount[];
  isLoading?: boolean;
}

export function AdminOrdersChart({
  ordersByMonth,
  isLoading = false,
}: AdminOrdersChartProps) {
  const chartData = toOrdersChartData(ordersByMonth);
  const hasData = chartData.some((item) => item.orders > 0);

  return (
    <Card className="h-full">
      <CardHeader>
        <CardTitle>Orders by month</CardTitle>
        <CardDescription>Order volume over the last 6 months</CardDescription>
      </CardHeader>
      <CardContent>
        {isLoading ? <Skeleton className="h-64 w-full rounded-xl" /> : null}

        {!isLoading && !hasData ? (
          <EmptyState
            icon={BarChart3}
            title="No order data"
            description="No order volume recorded for the last 6 months."
            className="py-8"
          />
        ) : null}

        {!isLoading && hasData ? (
          <ChartContainer config={chartConfig} className="h-64 w-full">
            <BarChart accessibilityLayer data={chartData}>
              <CartesianGrid vertical={false} />
              <XAxis dataKey="month" tickLine={false} axisLine={false} tickMargin={8} />
              <YAxis tickLine={false} axisLine={false} tickMargin={8} allowDecimals={false} />
              <Tooltip
                cursor={false}
                content={({ active, payload, label }) => {
                  if (!active || !payload?.length) return null;
                  const value = payload[0]?.value;
                  return (
                    <div className="rounded-lg border border-border bg-background px-3 py-2 text-xs">
                      <p className="mb-1 font-medium">{label}</p>
                      <p className="text-muted-foreground">
                        Orders:{" "}
                        <span className="font-medium text-foreground">
                          {Number(value ?? 0)}
                        </span>
                      </p>
                    </div>
                  );
                }}
              />
              <Bar dataKey="orders" fill="var(--color-orders)" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ChartContainer>
        ) : null}
      </CardContent>
    </Card>
  );
}
