"use client";

import { CheckCircle2, Eye, Loader2, XCircle } from "lucide-react";
import { useEffect, useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { AdminWithdrawalDetailModal } from "@/features/admin/withdrawals/components/admin-withdrawal-detail-modal";
import { AdminWithdrawalSearchFilter } from "@/features/admin/withdrawals/components/admin-withdrawal-search-filter";
import { useAdminWithdrawals } from "@/features/admin/withdrawals/hooks/use-admin-withdrawals";
import { useApproveWithdrawal } from "@/features/admin/withdrawals/hooks/use-approve-withdrawal";
import { useCompleteWithdrawal } from "@/features/admin/withdrawals/hooks/use-complete-withdrawal";
import { useRejectWithdrawal } from "@/features/admin/withdrawals/hooks/use-reject-withdrawal";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import { getWithdrawalStatusBadgeClass } from "@/features/withdrawals/lib/withdrawal-formatters";
import { WithdrawalStatusFilter } from "@/features/withdrawals/components/withdrawal-status-filter";
import { UsersPagination } from "@/features/admin/users/components/users-pagination";
import { isApiError } from "@/services/api-client";
import { WithdrawalStatus, type WithdrawalStatusFilter as WithdrawalStatusFilterType } from "@/types/withdrawal";

const PAGE_SIZE = 20;
const SEARCH_DEBOUNCE_MS = 400;

export function AdminWithdrawalsTable() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState<WithdrawalStatusFilterType>("all");
  const [userInput, setUserInput] = useState("");
  const [userSearch, setUserSearch] = useState("");
  const [selectedWithdrawalId, setSelectedWithdrawalId] = useState<string | null>(null);
  const [isDetailOpen, setIsDetailOpen] = useState(false);
  const [actionError, setActionError] = useState<string | null>(null);
  const [pendingActionId, setPendingActionId] = useState<string | null>(null);

  const approveWithdrawal = useApproveWithdrawal();
  const rejectWithdrawal = useRejectWithdrawal();
  const completeWithdrawal = useCompleteWithdrawal();

  useEffect(() => {
    const timer = window.setTimeout(() => {
      setUserSearch(userInput.trim());
      setPage(1);
    }, SEARCH_DEBOUNCE_MS);

    return () => window.clearTimeout(timer);
  }, [userInput]);

  const status = statusFilter === "all" ? undefined : statusFilter;

  const { data, isLoading, isError, error, isFetching } = useAdminWithdrawals({
    page,
    pageSize: PAGE_SIZE,
    user: userSearch || undefined,
    status,
  });

  const handleStatusFilterChange = (value: WithdrawalStatusFilterType) => {
    setStatusFilter(value);
    setPage(1);
  };

  const handleViewDetail = (withdrawalId: string) => {
    setActionError(null);
    setSelectedWithdrawalId(withdrawalId);
    setIsDetailOpen(true);
  };

  const handleDetailOpenChange = (open: boolean) => {
    setIsDetailOpen(open);

    if (!open) {
      setSelectedWithdrawalId(null);
      setActionError(null);
    }
  };

  const handleApprove = async (withdrawalId: string) => {
    setActionError(null);
    setPendingActionId(withdrawalId);

    try {
      await approveWithdrawal.mutateAsync(withdrawalId);
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    } finally {
      setPendingActionId(null);
    }
  };

  const handleReject = async (withdrawalId: string) => {
    setActionError(null);
    setPendingActionId(withdrawalId);

    try {
      await rejectWithdrawal.mutateAsync({ withdrawalId });
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    } finally {
      setPendingActionId(null);
    }
  };

  const handleComplete = async (withdrawalId: string) => {
    setActionError(null);
    setPendingActionId(withdrawalId);

    try {
      await completeWithdrawal.mutateAsync(withdrawalId);
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    } finally {
      setPendingActionId(null);
    }
  };

  const isActionPending =
    approveWithdrawal.isPending ||
    rejectWithdrawal.isPending ||
    completeWithdrawal.isPending;

  return (
    <>
      <Card>
        <CardHeader className="gap-4">
          <div>
            <CardTitle>Withdrawals</CardTitle>
            <CardDescription>
              Review withdrawal requests, filter by status, and process approvals.
            </CardDescription>
          </div>

          <AdminWithdrawalSearchFilter
            user={userInput}
            onUserChange={setUserInput}
            disabled={isLoading || isFetching}
          />

          <WithdrawalStatusFilter
            value={statusFilter}
            onChange={handleStatusFilterChange}
            disabled={isLoading || isFetching}
          />
        </CardHeader>

        <CardContent className="space-y-4">
          {actionError ? (
            <p className="text-sm text-destructive" role="alert">
              {actionError}
            </p>
          ) : null}

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
              No withdrawals found matching your filters.
            </p>
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <div className="overflow-x-auto rounded-lg border">
              <table className="w-full min-w-[900px] text-left text-sm">
                <thead className="border-b bg-muted/40">
                  <tr>
                    <th className="px-4 py-3 font-medium">User</th>
                    <th className="px-4 py-3 font-medium">Amount</th>
                    <th className="px-4 py-3 font-medium">Status</th>
                    <th className="px-4 py-3 font-medium">Requested date</th>
                    <th className="px-4 py-3 font-medium text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {data.items.map((withdrawal) => {
                    const isRowActionPending =
                      isActionPending && pendingActionId === withdrawal.id;

                    return (
                      <tr key={withdrawal.id} className="border-b last:border-b-0">
                        <td className="px-4 py-3">
                          <p className="font-medium">{withdrawal.user.fullName}</p>
                          <p className="text-xs text-muted-foreground">
                            {withdrawal.user.email}
                          </p>
                        </td>
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
                        <td className="px-4 py-3">
                          <div className="flex flex-wrap justify-end gap-2">
                            <Button
                              type="button"
                              variant="outline"
                              size="sm"
                              disabled={isRowActionPending}
                              onClick={() => handleViewDetail(withdrawal.id)}
                            >
                              <Eye />
                              View
                            </Button>

                            {withdrawal.status === WithdrawalStatus.Pending ? (
                              <>
                                <Button
                                  type="button"
                                  variant="default"
                                  size="sm"
                                  disabled={isRowActionPending}
                                  onClick={() => void handleApprove(withdrawal.id)}
                                >
                                  <CheckCircle2 />
                                  Approve
                                </Button>
                                <Button
                                  type="button"
                                  variant="destructive"
                                  size="sm"
                                  disabled={isRowActionPending}
                                  onClick={() => void handleReject(withdrawal.id)}
                                >
                                  <XCircle />
                                  Reject
                                </Button>
                              </>
                            ) : null}

                            {withdrawal.status === WithdrawalStatus.Approved ? (
                              <Button
                                type="button"
                                variant="default"
                                size="sm"
                                disabled={isRowActionPending}
                                onClick={() => void handleComplete(withdrawal.id)}
                              >
                                <CheckCircle2 />
                                Complete
                              </Button>
                            ) : null}
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : null}

          {!isLoading && !isError && data && data.totalCount > 0 ? (
            <UsersPagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
              disabled={isFetching}
            />
          ) : null}
        </CardContent>
      </Card>

      <AdminWithdrawalDetailModal
        withdrawalId={selectedWithdrawalId}
        open={isDetailOpen}
        onOpenChange={handleDetailOpenChange}
      />
    </>
  );
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to process withdrawal.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to process withdrawal.";
}
