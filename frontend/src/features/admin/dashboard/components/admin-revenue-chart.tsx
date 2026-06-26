"use client";

import { Bar, BarChart, CartesianGrid, Tooltip, XAxis, YAxis } from "recharts";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { ChartContainer } from "@/components/ui/chart";
import { toRevenueChartData } from "@/features/admin/dashboard/lib/chart-formatters";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import type { MonthlyRevenue } from "@/types/admin-dashboard";

const chartConfig = {
  revenue: {
    label: "Revenue",
    color: "var(--chart-3)",
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
        {isLoading ? (
          <div className="flex h-64 items-center justify-center text-sm text-muted-foreground">
            Loading chart...
          </div>
        ) : null}

        {!isLoading && !hasData ? (
          <div className="flex h-64 items-center justify-center text-sm text-muted-foreground">
            No revenue data for the last 6 months.
          </div>
        ) : null}

        {!isLoading && hasData ? (
          <ChartContainer config={chartConfig} className="h-64 w-full">
            <BarChart accessibilityLayer data={chartData}>
              <CartesianGrid vertical={false} />
              <XAxis
                dataKey="month"
                tickLine={false}
                axisLine={false}
                tickMargin={8}
              />
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
                  if (!active || !payload?.length) {
                    return null;
                  }

                  const value = payload[0]?.value;

                  return (
                    <div className="rounded-lg border bg-background px-3 py-2 text-xs shadow-xl">
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
              <Bar
                dataKey="revenue"
                fill="var(--color-revenue)"
                radius={[4, 4, 0, 0]}
              />
            </BarChart>
          </ChartContainer>
        ) : null}
      </CardContent>
    </Card>
  );
}
