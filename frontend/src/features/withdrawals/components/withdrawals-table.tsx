"use client";

import { Loader2 } from "lucide-react";
import { useState } from "react";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { OrdersPagination } from "@/features/orders/components/orders-pagination";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import { WithdrawalStatusFilter as WithdrawalStatusFilterBar } from "@/features/withdrawals/components/withdrawal-status-filter";
import { useWithdrawals } from "@/features/withdrawals/hooks/use-withdrawals";
import { getWithdrawalStatusBadgeClass } from "@/features/withdrawals/lib/withdrawal-formatters";
import { isApiError } from "@/services/api-client";
import { WithdrawalStatus, type WithdrawalStatusFilter } from "@/types/withdrawal";

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

  const handleStatusFilterChange = (value: WithdrawalStatusFilter) => {
    setStatusFilter(value);
    setPage(1);
  };

  return (
    <Card>
      <CardHeader className="gap-4">
        <div>
          <CardTitle>Withdrawal history</CardTitle>
          <CardDescription>
            Track the status of your withdrawal requests.
          </CardDescription>
        </div>

        <WithdrawalStatusFilterBar
          value={statusFilter}
          onChange={handleStatusFilterChange}
          disabled={isLoading || isFetching}
        />
      </CardHeader>

      <CardContent className="space-y-4">
        {isLoading ? (
          <div className="flex items-center justify-center py-12 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading withdrawals...
          </div>
        ) : null}

        {isError ? (
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        ) : null}

        {!isLoading && !isError && data?.items.length === 0 ? (
          <p className="py-12 text-center text-sm text-muted-foreground">
            {statusFilter === "all"
              ? "No withdrawal requests yet. Submit a request above to see your history here."
              : `No ${getFilterEmptyLabel(statusFilter)} withdrawals found.`}
          </p>
        ) : null}

        {!isLoading && !isError && data && data.items.length > 0 ? (
          <div className="overflow-x-auto rounded-lg border">
            <table className="w-full min-w-[640px] text-left text-sm">
              <thead className="border-b bg-muted/40">
                <tr>
                  <th className="px-4 py-3 font-medium">Amount</th>
                  <th className="px-4 py-3 font-medium">Status</th>
                  <th className="px-4 py-3 font-medium">Requested date</th>
                  <th className="px-4 py-3 font-medium">Processed date</th>
                </tr>
              </thead>
              <tbody>
                {data.items.map((withdrawal) => (
                  <tr key={withdrawal.id} className="border-b last:border-b-0">
                    <td className="px-4 py-3 font-medium">
                      {formatCurrency(withdrawal.amount)}
                    </td>
                    <td className="px-4 py-3">
                      <span
                        className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getWithdrawalStatusBadgeClass(withdrawal.status)}`}
                      >
                        {withdrawal.statusName}
                      </span>
                    </td>
                    <td className="whitespace-nowrap px-4 py-3 text-muted-foreground">
                      {formatDateTime(withdrawal.requestedAt)}
                    </td>
                    <td className="whitespace-nowrap px-4 py-3 text-muted-foreground">
                      {withdrawal.processedAt
                        ? formatDateTime(withdrawal.processedAt)
                        : "—"}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : null}

        {!isLoading && !isError && data && data.totalCount > 0 ? (
          <OrdersPagination
            page={data.page}
            pageSize={data.pageSize}
            totalCount={data.totalCount}
            onPageChange={setPage}
            disabled={isFetching}
          />
        ) : null}
      </CardContent>
    </Card>
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
