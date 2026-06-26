"use client";

import Link from "next/link";
import { Loader2 } from "lucide-react";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import {
  formatDateTime,
} from "@/features/admin/users/lib/user-formatters";
import { getWithdrawalStatusBadgeClass } from "@/features/withdrawals/lib/withdrawal-formatters";
import type { AdminRecentWithdrawal } from "@/types/admin-dashboard";

interface AdminRecentWithdrawalsWidgetProps {
  withdrawals: AdminRecentWithdrawal[];
  isLoading?: boolean;
}

export function AdminRecentWithdrawalsWidget({
  withdrawals,
  isLoading = false,
}: AdminRecentWithdrawalsWidgetProps) {
  return (
    <Card className="h-full">
      <CardHeader className="flex flex-col gap-2 sm:flex-row sm:items-start sm:justify-between">
        <div>
          <CardTitle>Recent withdrawals</CardTitle>
          <CardDescription>Latest payout requests</CardDescription>
        </div>
        <Link
          href="/admin/withdrawals"
          className="text-sm font-medium text-primary hover:underline"
        >
          View all
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? (
          <div className="flex items-center justify-center py-10 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading recent withdrawals...
          </div>
        ) : null}

        {!isLoading && withdrawals.length === 0 ? (
          <p className="py-10 text-center text-sm text-muted-foreground">
            No recent withdrawals yet.
          </p>
        ) : null}

        {!isLoading && withdrawals.length > 0 ? (
          <div className="space-y-3">
            {withdrawals.map((withdrawal) => (
              <div
                key={withdrawal.id}
                className="flex items-start justify-between gap-3 rounded-lg border bg-muted/20 px-3 py-2"
              >
                <div className="min-w-0">
                  <p className="truncate text-sm font-medium">
                    {formatCurrency(withdrawal.amount)}
                  </p>
                  <p className="truncate text-xs text-muted-foreground">
                    {withdrawal.userEmail}
                  </p>
                  <p className="mt-1 text-xs text-muted-foreground">
                    {formatDateTime(withdrawal.requestedAt)}
                  </p>
                </div>
                <span
                  className={`shrink-0 rounded-full px-2 py-0.5 text-xs font-medium ${getWithdrawalStatusBadgeClass(withdrawal.status)}`}
                >
                  {withdrawal.statusName}
                </span>
              </div>
            ))}
          </div>
        ) : null}
      </CardContent>
    </Card>
  );
}
