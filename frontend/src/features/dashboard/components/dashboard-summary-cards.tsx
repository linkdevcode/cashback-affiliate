"use client";

import Link from "next/link";
import { Loader2, Wallet } from "lucide-react";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { buttonVariants } from "@/components/ui/button";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";
import { emptyDashboardSummary, type DashboardSummary } from "@/types/dashboard";
import { cn } from "@/lib/utils";

interface DashboardSummaryCardsProps {
  summary?: DashboardSummary;
  isLoading?: boolean;
  isError?: boolean;
  error?: unknown;
}

export function DashboardSummaryCards({
  summary = emptyDashboardSummary,
  isLoading = false,
  isError = false,
  error,
}: DashboardSummaryCardsProps) {
  if (isLoading) {
    return <DashboardSummaryCardsSkeleton />;
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

  if (isDashboardEmpty(summary)) {
    return <DashboardEmptyState />;
  }

  const cards = [
    {
      title: "Available Balance",
      description: "Ready to withdraw",
      value: summary.availableBalance,
    },
    {
      title: "Pending Cashback",
      description: "Awaiting approval",
      value: summary.pendingCashback,
    },
    {
      title: "Total Cashback",
      description: "Approved and pending",
      value: summary.totalCashback,
    },
    {
      title: "Total Withdrawn",
      description: "Completed payouts",
      value: summary.totalWithdrawn,
    },
  ];

  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
      {cards.map((card) => (
        <Card key={card.title}>
          <CardHeader className="pb-2">
            <CardDescription>{card.description}</CardDescription>
            <CardTitle className="text-base font-medium">{card.title}</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-2xl font-semibold tracking-tight sm:text-xl lg:text-2xl">
              {formatCurrency(card.value)}
            </p>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

function DashboardSummaryCardsSkeleton() {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
      {Array.from({ length: 4 }).map((_, index) => (
        <Card key={index}>
          <CardContent className="flex items-center justify-center py-10">
            <Loader2 className="size-4 animate-spin text-muted-foreground" />
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

function DashboardEmptyState() {
  return (
    <Card>
      <CardContent className="flex flex-col items-center justify-center gap-4 py-12 text-center">
        <div className="flex size-12 items-center justify-center rounded-full bg-muted">
          <Wallet className="size-6 text-muted-foreground" />
        </div>
        <div className="space-y-1">
          <p className="text-base font-medium">No earnings yet</p>
          <p className="max-w-sm text-sm text-muted-foreground">
            Generate an affiliate link and make a purchase to start earning cashback.
          </p>
        </div>
        <Link
          href="/dashboard/affiliate"
          className={cn(buttonVariants({ variant: "outline" }))}
        >
          Create affiliate link
        </Link>
      </CardContent>
    </Card>
  );
}

function isDashboardEmpty(summary: DashboardSummary): boolean {
  const hasChartData = summary.cashbackByMonth.some(
    (item) => item.cashbackAmount > 0,
  );

  return (
    summary.availableBalance === 0 &&
    summary.pendingCashback === 0 &&
    summary.totalCashback === 0 &&
    summary.totalWithdrawn === 0 &&
    summary.recentOrders.length === 0 &&
    !hasChartData
  );
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load dashboard summary.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load dashboard summary.";
}
