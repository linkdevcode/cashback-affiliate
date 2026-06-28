"use client";

import { Banknote } from "lucide-react";
import { useState } from "react";

import { EmptyState } from "@/components/empty-state";
import { WithdrawalStatusBadge } from "@/components/status-badge";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { OrdersPagination } from "@/features/orders/components/orders-pagination";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import {
  hasWithdrawalFiltersActive,
  WithdrawalFilters,
} from "@/features/withdrawals/components/withdrawal-filters";
import { useWithdrawals } from "@/features/withdrawals/hooks/use-withdrawals";
import {
  formatWithdrawalReference,
  getWithdrawalStatusDescription,
} from "@/features/withdrawals/lib/withdrawal-formatters";
import { cn } from "@/lib/utils";
import { isApiError } from "@/services/api-client";
import {
  WithdrawalStatus,
  type WithdrawalListItem,
  type WithdrawalStatusFilter,
} from "@/types/withdrawal";

const PAGE_SIZE = 20;

export function WithdrawalsTable() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState<WithdrawalStatusFilter>("all");

  const status = statusFilter === "all" ? undefined : statusFilter;

  const { data, isLoading, isError, error, isFetching } = useWithdrawals({
    page,
    pageSize: PAGE_SIZE,
    status,
  });

  const hasActiveFilters = hasWithdrawalFiltersActive(statusFilter);
  const isEmpty = !isLoading && !isError && data?.items.length === 0;

  const handleStatusFilterChange = (value: WithdrawalStatusFilter) => {
    setStatusFilter(value);
    setPage(1);
  };

  const handleClearFilters = () => {
    setStatusFilter("all");
    setPage(1);
  };

  return (
    <Card>
      <CardHeader className="gap-4">
        <div>
          <CardTitle>Payout history</CardTitle>
          <CardDescription>
            Your withdrawal requests and bank transfer status — auditable records.
          </CardDescription>
        </div>

        <WithdrawalFilters
          statusFilter={statusFilter}
          onStatusFilterChange={handleStatusFilterChange}
          onClearFilters={handleClearFilters}
          disabled={isLoading}
          hasActiveFilters={hasActiveFilters}
        />
      </CardHeader>

      <CardContent className="space-y-4">
        {isError ? (
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        ) : null}

        {isLoading ? <WithdrawalsTableSkeleton /> : null}

        {isEmpty && !hasActiveFilters ? (
          <EmptyState
            icon={Banknote}
            title="No payouts yet"
            description="Submit a withdrawal request to see payout records here."
          />
        ) : null}

        {isEmpty && hasActiveFilters ? (
          <EmptyState
            icon={Banknote}
            title="No matching withdrawals"
            description={`No ${getFilterEmptyLabel(statusFilter)} withdrawals found.`}
            action={
              <Button
                type="button"
                variant="ghost"
                size="sm"
                onClick={handleClearFilters}
              >
                Clear filters
              </Button>
            }
          />
        ) : null}

        {!isLoading && !isError && data && data.items.length > 0 ? (
            <div className="relative">
            {isFetching ? (
              <div
                className="absolute inset-x-0 top-0 z-10 h-0.5 overflow-hidden rounded-full bg-muted"
                aria-hidden="true"
              >
                <div className="h-full w-1/3 animate-pulse bg-brand" />
              </div>
            ) : null}

            <div
              className={cn(
                "transition-opacity",
                isFetching && "pointer-events-none opacity-60",
              )}
            >
              <WithdrawalsDesktopTable withdrawals={data.items} />
              <WithdrawalsMobileList withdrawals={data.items} />
            </div>

            {isFetching ? (
              <p className="mt-2 text-xs text-muted-foreground">Updating…</p>
            ) : null}
          </div>
        ) : null}
      </CardContent>

      {!isLoading && !isError && data && data.totalCount > 0 ? (
        <CardFooter className="border-t">
          <OrdersPagination
            page={data.page}
            pageSize={data.pageSize}
            totalCount={data.totalCount}
            onPageChange={setPage}
            disabled={isFetching}
          />
        </CardFooter>
      ) : null}
    </Card>
  );
}

interface WithdrawalsListProps {
  withdrawals: WithdrawalListItem[];
}

function WithdrawalsDesktopTable({ withdrawals }: WithdrawalsListProps) {
  return (
    <div className="hidden overflow-hidden rounded-xl border border-border md:block">
      <table className="w-full text-left text-sm">
        <thead className="sticky top-0 z-10 border-b border-border bg-muted/40">
          <tr>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Reference
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground text-right">
              Amount
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Status
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Requested
            </th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">
              Processed
            </th>
          </tr>
        </thead>
        <tbody>
          {withdrawals.map((withdrawal) => (
            <tr
              key={withdrawal.id}
              className="border-b border-border last:border-b-0 hover:bg-muted/30"
            >
              <td className="px-4 py-3 font-mono text-xs text-muted-foreground">
                {formatWithdrawalReference(withdrawal.id)}
              </td>
              <td className="px-4 py-3 text-base font-semibold tabular-nums">
                {formatCurrency(withdrawal.amount)}
              </td>
              <td className="px-4 py-3">
                <WithdrawalStatusCell withdrawal={withdrawal} />
              </td>
              <td className="px-4 py-3 text-xs text-muted-foreground">
                {formatDateTime(withdrawal.requestedAt)}
              </td>
              <td className="px-4 py-3 text-xs text-muted-foreground">
                {withdrawal.processedAt
                  ? formatDateTime(withdrawal.processedAt)
                  : "—"}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function WithdrawalsMobileList({ withdrawals }: WithdrawalsListProps) {
  return (
    <div className="space-y-3 md:hidden">
      {withdrawals.map((withdrawal) => (
        <article
          key={withdrawal.id}
          className="space-y-3 rounded-xl border border-border p-4"
        >
          <div className="flex items-start justify-between gap-3">
            <div className="space-y-1">
              <p className="text-xl font-semibold tabular-nums">
                {formatCurrency(withdrawal.amount)}
              </p>
              <p className="font-mono text-xs text-muted-foreground">
                Ref {formatWithdrawalReference(withdrawal.id)}
              </p>
            </div>
            <WithdrawalStatusBadge
              status={withdrawal.status}
              label={withdrawal.statusName}
              className="shrink-0"
            />
          </div>

          <p className="text-xs text-muted-foreground">
            {getWithdrawalStatusDescription(withdrawal.status)}
          </p>

          <dl className="grid grid-cols-2 gap-3 border-t border-border pt-3">
            <div>
              <dt className="text-xs text-muted-foreground">Requested</dt>
              <dd className="text-xs text-muted-foreground">
                {formatDateTime(withdrawal.requestedAt)}
              </dd>
            </div>
            <div>
              <dt className="text-xs text-muted-foreground">Processed</dt>
              <dd className="text-xs text-muted-foreground">
                {withdrawal.processedAt
                  ? formatDateTime(withdrawal.processedAt)
                  : "—"}
              </dd>
            </div>
          </dl>
        </article>
      ))}
    </div>
  );
}

function WithdrawalStatusCell({ withdrawal }: { withdrawal: WithdrawalListItem }) {
  return (
    <div className="space-y-1">
      <WithdrawalStatusBadge status={withdrawal.status} label={withdrawal.statusName} />
      <p className="text-xs text-muted-foreground">
        {getWithdrawalStatusDescription(withdrawal.status)}
      </p>
    </div>
  );
}

function WithdrawalsTableSkeleton() {
  return (
    <>
      <div className="hidden overflow-hidden rounded-xl border border-border md:block">
        <div className="border-b border-border bg-muted/40 px-4 py-3">
          <Skeleton className="h-3 w-full max-w-md" />
        </div>
        {Array.from({ length: 5 }).map((_, index) => (
          <div
            key={index}
            className="flex gap-4 border-b border-border px-4 py-3 last:border-b-0"
          >
            <Skeleton className="h-4 w-16" />
            <Skeleton className="h-5 w-24" />
            <Skeleton className="h-4 w-28" />
            <Skeleton className="h-4 w-28" />
            <Skeleton className="h-4 w-28" />
          </div>
        ))}
      </div>
      <div className="space-y-3 md:hidden">
        {Array.from({ length: 3 }).map((_, index) => (
          <Skeleton key={index} className="h-32 w-full rounded-xl" />
        ))}
      </div>
    </>
  );
}

function getFilterEmptyLabel(status: WithdrawalStatusFilter): string {
  switch (status) {
    case WithdrawalStatus.Pending:
      return "pending";
    case WithdrawalStatus.Approved:
      return "approved";
    case WithdrawalStatus.Rejected:
      return "rejected";
    case WithdrawalStatus.Completed:
      return "completed";
    default:
      return "";
  }
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load withdrawals.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load withdrawals.";
}
