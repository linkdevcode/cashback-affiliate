"use client";

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
import { toChartData } from "@/features/dashboard/lib/chart-formatters";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import type { MonthlyCashback } from "@/types/dashboard";
import { BarChart3 } from "lucide-react";

const chartConfig = {
  cashback: {
    label: "Cashback",
    color: "var(--chart-cashback)",
  },
};

interface EarningsChartProps {
  cashbackByMonth: MonthlyCashback[];
  isLoading?: boolean;
}

export function EarningsChart({ cashbackByMonth, isLoading = false }: EarningsChartProps) {
  const chartData = toChartData(cashbackByMonth);
  const hasData = chartData.some((item) => item.cashback > 0);

  return (
    <Card className="h-full">
      <CardHeader>
        <CardTitle className="text-lg font-semibold">Earnings</CardTitle>
        <CardDescription>Cashback by month for the last 6 months</CardDescription>
      </CardHeader>
      <CardContent>
        {isLoading ? <EarningsChartSkeleton /> : null}

        {!isLoading && !hasData ? (
          <EmptyState
            icon={BarChart3}
            title="No earnings data yet"
            description="Your monthly cashback chart will appear here once orders are approved."
          />
        ) : null}

        {!isLoading && hasData ? (
          <ChartContainer config={chartConfig} className="h-64 w-full">
            <BarChart accessibilityLayer data={chartData}>
              <CartesianGrid vertical={false} stroke="var(--border)" />
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
                    <div className="rounded-lg border border-border bg-background px-3 py-2 text-xs">
                      <p className="mb-1 font-medium">{label}</p>
                      <p className="text-muted-foreground">
                        Cashback:{" "}
                        <span className="font-medium tabular-nums text-foreground">
                          {formatCurrency(Number(value ?? 0))}
                        </span>
                      </p>
                    </div>
                  );
                }}
              />
              <Bar
                dataKey="cashback"
                fill="var(--color-cashback)"
                radius={[4, 4, 0, 0]}
              />
            </BarChart>
          </ChartContainer>
        ) : null}
      </CardContent>
    </Card>
  );
}

function EarningsChartSkeleton() {
  return (
    <div className="flex h-64 flex-col justify-end gap-2">
      <div className="flex h-full items-end justify-between gap-2 px-2">
        {Array.from({ length: 6 }).map((_, index) => (
          <Skeleton
            key={index}
            className="w-full"
            style={{ height: `${40 + (index % 3) * 20}%` }}
          />
        ))}
      </div>
      <Skeleton className="h-3 w-full" />
    </div>
  );
}
