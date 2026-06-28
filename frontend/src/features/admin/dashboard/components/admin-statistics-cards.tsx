"use client";

import Link from "next/link";
import { AlertCircle } from "lucide-react";

import { AdminStatBlock } from "@/components/admin-stat-block";
import { buttonVariants } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
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
        { label: "Active users", value: formatCount(summary.activeUsers), featured: true },
        { label: "Suspended users", value: formatCount(summary.suspendedUsers) },
      ],
    },
    {
      title: "Order statistics",
      description: "Affiliate conversion activity",
      metrics: [
        { label: "Total orders", value: formatCount(summary.totalOrders) },
        { label: "Pending orders", value: formatCount(summary.pendingOrders), featured: true },
        { label: "Approved orders", value: formatCount(summary.approvedOrders) },
        { label: "Rejected orders", value: formatCount(summary.rejectedOrders) },
      ],
    },
    {
      title: "Withdrawal statistics",
      description: "Payout request activity",
      metrics: [
        { label: "Total withdrawals", value: formatCount(summary.totalWithdrawals) },
        {
          label: "Pending withdrawals",
          value: formatCount(summary.pendingWithdrawals),
          featured: true,
        },
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
        {
          label: "Platform revenue",
          value: formatCurrency(summary.platformRevenue),
          featured: true,
        },
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
                <AdminStatBlock
                  key={metric.label}
                  label={metric.label}
                  value={metric.value}
                  featured={"featured" in metric && metric.featured}
                />
              ))}
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

interface AdminPendingActionsBannerProps {
  pendingWithdrawals: number;
}

export function AdminPendingActionsBanner({
  pendingWithdrawals,
}: AdminPendingActionsBannerProps) {
  if (pendingWithdrawals <= 0) {
    return null;
  }

  return (
    <Card className="border-l-[3px] border-l-warning bg-warning-muted">
      <CardContent className="flex flex-col gap-3 py-4 sm:flex-row sm:items-center sm:justify-between">
        <div className="flex gap-3">
          <AlertCircle className="mt-0.5 size-5 shrink-0 text-warning" aria-hidden="true" />
          <div className="space-y-1">
            <p className="text-sm font-medium">
              {formatCount(pendingWithdrawals)} withdrawal
              {pendingWithdrawals === 1 ? "" : "s"} awaiting review
            </p>
            <p className="text-xs text-muted-foreground">
              Pending payouts need admin approval before bank transfer.
            </p>
          </div>
        </div>
        <Link
          href="/admin/withdrawals"
          className={buttonVariants({ variant: "outline", size: "sm" })}
        >
          Review withdrawals
        </Link>
      </CardContent>
    </Card>
  );
}

function AdminStatisticsCardsSkeleton() {
  return (
    <div className="grid grid-cols-1 gap-4 xl:grid-cols-2">
      {Array.from({ length: 4 }).map((_, index) => (
        <Card key={index}>
          <CardHeader className="space-y-2 pb-3">
            <Skeleton className="h-4 w-32" />
            <Skeleton className="h-3 w-48" />
          </CardHeader>
          <CardContent>
            <div className="grid gap-3 sm:grid-cols-2">
              {Array.from({ length: 3 }).map((__, metricIndex) => (
                <Skeleton key={metricIndex} className="h-14 w-full rounded-lg" />
              ))}
            </div>
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
