"use client";

import Link from "next/link";
import { Wallet } from "lucide-react";

import { EmptyState } from "@/components/empty-state";
import { WithdrawalStatusBadge } from "@/components/status-badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { formatDateTime } from "@/features/admin/users/lib/user-formatters";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
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
        <div className="space-y-1">
          <CardTitle className="text-lg font-semibold">Recent withdrawals</CardTitle>
          <CardDescription>Latest payout requests</CardDescription>
        </div>
        <Link href="/admin/withdrawals" className="text-sm font-medium text-brand hover:underline">
          View all
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? <RecentWithdrawalsSkeleton /> : null}

        {!isLoading && withdrawals.length === 0 ? (
          <EmptyState
            icon={Wallet}
            title="No recent withdrawals"
            description="New payout requests will appear here."
          />
        ) : null}

        {!isLoading && withdrawals.length > 0 ? (
          <div className="space-y-3">
            {withdrawals.map((withdrawal) => (
              <div
                key={withdrawal.id}
                className="flex items-start justify-between gap-3 rounded-lg bg-muted/30 px-3 py-2"
              >
                <div className="min-w-0">
                  <p className="text-sm font-semibold tabular-nums">
                    {formatCurrency(withdrawal.amount)}
                  </p>
                  <p className="truncate text-xs text-muted-foreground">
                    {withdrawal.userEmail}
                  </p>
                  <p className="mt-1 text-xs text-muted-foreground">
                    {formatDateTime(withdrawal.requestedAt)}
                  </p>
                </div>
                <WithdrawalStatusBadge
                  status={withdrawal.status}
                  label={withdrawal.statusName}
                  className="shrink-0"
                />
              </div>
            ))}
          </div>
        ) : null}
      </CardContent>
    </Card>
  );
}

function RecentWithdrawalsSkeleton() {
  return (
    <div className="space-y-3">
      {Array.from({ length: 4 }).map((_, index) => (
        <Skeleton key={index} className="h-16 w-full rounded-lg" />
      ))}
    </div>
  );
}
