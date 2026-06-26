"use client";

import { Loader2 } from "lucide-react";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";
import {
  emptyAdminDashboardSummary,
  type AdminDashboardSummary,
} from "@/types/admin-dashboard";

interface AdminStatisticsCardsProps {
  summary?: AdminDashboardSummary;
  isLoading?: boolean;
  isError?: boolean;
  error?: unknown;
}

export function AdminStatisticsCards({
  summary = emptyAdminDashboardSummary,
  isLoading = false,
  isError = false,
  error,
}: AdminStatisticsCardsProps) {
  if (isLoading) {
    return <AdminStatisticsCardsSkeleton />;
  }

  if (isError) {
    return (
      <Card>
        <CardContent className="py-6">
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        </CardContent>
      </Card>
    );
  }

  const sections = [
    {
      title: "User statistics",
      description: "Platform user accounts",
      metrics: [
        { label: "Total users", value: formatCount(summary.totalUsers) },
        { label: "Active users", value: formatCount(summary.activeUsers) },
        { label: "Suspended users", value: formatCount(summary.suspendedUsers) },
      ],
    },
    {
      title: "Order statistics",
      description: "Affiliate conversion activity",
      metrics: [
        { label: "Total orders", value: formatCount(summary.totalOrders) },
        { label: "Pending orders", value: formatCount(summary.pendingOrders) },
        { label: "Approved orders", value: formatCount(summary.approvedOrders) },
        { label: "Rejected orders", value: formatCount(summary.rejectedOrders) },
      ],
    },
    {
      title: "Withdrawal statistics",
      description: "Payout request activity",
      metrics: [
        { label: "Total withdrawals", value: formatCount(summary.totalWithdrawals) },
        { label: "Pending withdrawals", value: formatCount(summary.pendingWithdrawals) },
        {
          label: "Completed withdrawals",
          value: formatCount(summary.completedWithdrawals),
        },
      ],
    },
    {
      title: "Revenue statistics",
      description: "Approved order financials",
      metrics: [
        { label: "Total commission", value: formatCurrency(summary.totalCommission) },
        { label: "Cashback paid", value: formatCurrency(summary.totalCashbackPaid) },
        { label: "Platform revenue", value: formatCurrency(summary.platformRevenue) },
      ],
    },
  ];

  return (
    <div className="grid grid-cols-1 gap-4 xl:grid-cols-2">
      {sections.map((section) => (
        <Card key={section.title}>
          <CardHeader className="pb-3">
            <CardTitle className="text-base">{section.title}</CardTitle>
            <CardDescription>{section.description}</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-3 sm:grid-cols-2">
              {section.metrics.map((metric) => (
                <div
                  key={metric.label}
                  className="rounded-lg border bg-muted/30 px-3 py-2"
                >
                  <p className="text-xs text-muted-foreground">{metric.label}</p>
                  <p className="mt-1 text-lg font-semibold tracking-tight">
                    {metric.value}
                  </p>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

function AdminStatisticsCardsSkeleton() {
  return (
    <div className="grid grid-cols-1 gap-4 xl:grid-cols-2">
      {Array.from({ length: 4 }).map((_, index) => (
        <Card key={index}>
          <CardContent className="flex items-center justify-center py-12">
            <Loader2 className="size-4 animate-spin text-muted-foreground" />
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

function formatCount(value: number): string {
  return new Intl.NumberFormat("vi-VN").format(value);
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load admin dashboard.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load admin dashboard.";
}
