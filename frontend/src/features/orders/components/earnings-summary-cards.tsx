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
import { useOrderSummary } from "@/features/orders/hooks/use-order-summary";
import { isApiError } from "@/services/api-client";

export function EarningsSummaryCards() {
  const { data, isLoading, isError, error } = useOrderSummary();

  if (isLoading) {
    return (
      <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
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

  const summary = data ?? {
    totalCashback: 0,
    pendingCashback: 0,
    approvedCashback: 0,
    rejectedCashback: 0,
  };

  const cards = [
    {
      title: "Total Cashback",
      description: "Across all orders",
      value: summary.totalCashback,
    },
    {
      title: "Pending Cashback",
      description: "Awaiting approval",
      value: summary.pendingCashback,
    },
    {
      title: "Approved Cashback",
      description: "Confirmed earnings",
      value: summary.approvedCashback,
    },
    {
      title: "Rejected Cashback",
      description: "From rejected orders",
      value: summary.rejectedCashback,
    },
  ];

  return (
    <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
      {cards.map((card) => (
        <Card key={card.title}>
          <CardHeader className="pb-2">
            <CardDescription>{card.description}</CardDescription>
            <CardTitle className="text-base font-medium">{card.title}</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-2xl font-semibold tracking-tight">
              {formatCurrency(card.value)}
            </p>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load earnings summary.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load earnings summary.";
}
