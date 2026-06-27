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
import { toRevenueChartData } from "@/features/admin/dashboard/lib/chart-formatters";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import type { MonthlyRevenue } from "@/types/admin-dashboard";

const chartConfig = {
  revenue: {
    label: "Revenue",
    color: "var(--chart-revenue)",
  },
};

interface AdminRevenueChartProps {
  revenueByMonth: MonthlyRevenue[];
  isLoading?: boolean;
}

export function AdminRevenueChart({
  revenueByMonth,
  isLoading = false,
}: AdminRevenueChartProps) {
  const chartData = toRevenueChartData(revenueByMonth);
  const hasData = chartData.some((item) => item.revenue > 0);

  return (
    <Card className="h-full">
      <CardHeader>
        <CardTitle>Revenue by month</CardTitle>
        <CardDescription>Platform revenue over the last 6 months</CardDescription>
      </CardHeader>
      <CardContent>
        {isLoading ? <Skeleton className="h-64 w-full rounded-xl" /> : null}

        {!isLoading && !hasData ? (
          <EmptyState
            icon={BarChart3}
            title="No revenue data"
            description="No platform revenue recorded for the last 6 months."
            className="py-8"
          />
        ) : null}

        {!isLoading && hasData ? (
          <ChartContainer config={chartConfig} className="h-64 w-full">
            <BarChart accessibilityLayer data={chartData}>
              <CartesianGrid vertical={false} />
              <XAxis dataKey="month" tickLine={false} axisLine={false} tickMargin={8} />
              <YAxis
                tickLine={false}
                axisLine={false}
                tickMargin={8}
                tickFormatter={(value: number) =>
                  new Intl.NumberFormat("vi-VN", {
                    notation: "compact",
                    maximumFractionDigits: 0,
                  }).format(value)
                }
              />
              <Tooltip
                cursor={false}
                content={({ active, payload, label }) => {
                  if (!active || !payload?.length) return null;
                  const value = payload[0]?.value;
                  return (
                    <div className="rounded-lg border border-border bg-background px-3 py-2 text-xs">
                      <p className="mb-1 font-medium">{label}</p>
                      <p className="text-muted-foreground">
                        Revenue:{" "}
                        <span className="font-medium text-foreground">
                          {formatCurrency(Number(value ?? 0))}
                        </span>
                      </p>
                    </div>
                  );
                }}
              />
              <Bar dataKey="revenue" fill="var(--color-revenue)" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ChartContainer>
        ) : null}
      </CardContent>
    </Card>
  );
}
