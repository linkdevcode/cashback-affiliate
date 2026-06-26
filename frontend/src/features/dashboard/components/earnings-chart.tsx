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
import { toChartData } from "@/features/dashboard/lib/chart-formatters";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import type { MonthlyCashback } from "@/types/dashboard";

const chartConfig = {
  cashback: {
    label: "Cashback",
    color: "var(--chart-2)",
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
        <CardTitle>Earnings Chart</CardTitle>
        <CardDescription>Cashback by month for the last 6 months</CardDescription>
      </CardHeader>
      <CardContent>
        {isLoading ? (
          <div className="flex h-64 items-center justify-center text-sm text-muted-foreground">
            Loading chart...
          </div>
        ) : null}

        {!isLoading && !hasData ? (
          <div className="flex h-64 items-center justify-center text-sm text-muted-foreground">
            No cashback data for the last 6 months.
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
                        Cashback:{" "}
                        <span className="font-medium text-foreground">
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
