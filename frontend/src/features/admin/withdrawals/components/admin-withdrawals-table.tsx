"use client";

import { CheckCircle2, Wallet } from "lucide-react";
import { useEffect, useState } from "react";

import { ConfirmDialog } from "@/components/confirm-dialog";
import { EmptyState } from "@/components/empty-state";
import { RowActionsMenu } from "@/components/row-actions-menu";
import { WithdrawalStatusBadge } from "@/components/status-badge";
import { TableFetchOverlay } from "@/components/table-fetch-overlay";
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
import { AdminWithdrawalDetailModal } from "@/features/admin/withdrawals/components/admin-withdrawal-detail-modal";
import { AdminWithdrawalFilters } from "@/features/admin/withdrawals/components/admin-withdrawal-filters";
import { useAdminWithdrawals } from "@/features/admin/withdrawals/hooks/use-admin-withdrawals";
import { useApproveWithdrawal } from "@/features/admin/withdrawals/hooks/use-approve-withdrawal";
import { useCompleteWithdrawal } from "@/features/admin/withdrawals/hooks/use-complete-withdrawal";
import { useRejectWithdrawal } from "@/features/admin/withdrawals/hooks/use-reject-withdrawal";
import { OrdersPagination } from "@/features/orders/components/orders-pagination";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import { getWithdrawalStatusDescription } from "@/features/withdrawals/lib/withdrawal-formatters";
import { isApiError } from "@/services/api-client";
import type { AdminWithdrawalListItem } from "@/types/admin-withdrawal";
import {
  WithdrawalStatus,
  type WithdrawalStatusFilter as WithdrawalStatusFilterType,
} from "@/types/withdrawal";

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
  const [confirmAction, setConfirmAction] = useState<{
    withdrawalId: string;
    type: "reject" | "complete";
  } | null>(null);

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

  const handleClearFilters = () => {
    setStatusFilter("all");
    setUserInput("");
    setUserSearch("");
    setPage(1);
  };

  const handleViewDetail = (withdrawalId: string) => {
    setActionError(null);
    setSelectedWithdrawalId(withdrawalId);
    setIsDetailOpen(true);
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

  const handleConfirmAction = async () => {
    if (!confirmAction) {
      return;
    }

    setActionError(null);
    setPendingActionId(confirmAction.withdrawalId);

    try {
      if (confirmAction.type === "reject") {
        await rejectWithdrawal.mutateAsync({ withdrawalId: confirmAction.withdrawalId });
      } else {
        await completeWithdrawal.mutateAsync(confirmAction.withdrawalId);
      }
      setConfirmAction(null);
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

  const hasActiveFilters = Boolean(userSearch) || statusFilter !== "all";
  const isEmpty = !isLoading && !isError && data?.items.length === 0;

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

          <AdminWithdrawalFilters
            user={userInput}
            statusFilter={statusFilter}
            onUserChange={setUserInput}
            onStatusFilterChange={(value) => {
              setStatusFilter(value);
              setPage(1);
            }}
            onClearFilters={handleClearFilters}
            disabled={isLoading}
          />
        </CardHeader>

        <CardContent className="space-y-4">
          {actionError ? (
            <p className="text-sm text-destructive" role="alert">
              {actionError}
            </p>
          ) : null}

          {isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(error)}
            </p>
          ) : null}

          {isLoading ? <AdminWithdrawalsTableSkeleton /> : null}

          {isEmpty && !hasActiveFilters ? (
            <EmptyState
              icon={Wallet}
              title="No withdrawals yet"
              description="Withdrawal requests will appear here when users submit payout requests."
            />
          ) : null}

          {isEmpty && hasActiveFilters ? (
            <EmptyState
              icon={Wallet}
              title="No matching withdrawals"
              description="No withdrawals found matching your filters."
              action={
                <Button type="button" variant="ghost" size="sm" onClick={handleClearFilters}>
                  Clear filters
                </Button>
              }
            />
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <TableFetchOverlay isFetching={isFetching}>
              <AdminWithdrawalsDesktopTable
                withdrawals={data.items}
                pendingActionId={pendingActionId}
                isActionPending={isActionPending}
                onViewDetail={handleViewDetail}
                onApprove={handleApprove}
                onRequestReject={(id) => setConfirmAction({ withdrawalId: id, type: "reject" })}
                onRequestComplete={(id) => setConfirmAction({ withdrawalId: id, type: "complete" })}
              />
              <AdminWithdrawalsMobileList
                withdrawals={data.items}
                pendingActionId={pendingActionId}
                isActionPending={isActionPending}
                onViewDetail={handleViewDetail}
                onApprove={handleApprove}
                onRequestReject={(id) => setConfirmAction({ withdrawalId: id, type: "reject" })}
                onRequestComplete={(id) => setConfirmAction({ withdrawalId: id, type: "complete" })}
              />
            </TableFetchOverlay>
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

      <AdminWithdrawalDetailModal
        withdrawalId={selectedWithdrawalId}
        open={isDetailOpen}
        onOpenChange={(open) => {
          if (!open) {
            setSelectedWithdrawalId(null);
            setActionError(null);
          }
          setIsDetailOpen(open);
        }}
      />

      <ConfirmDialog
        open={confirmAction?.type === "reject"}
        onOpenChange={(open) => !open && setConfirmAction(null)}
        title="Reject withdrawal?"
        description="The user's balance will be restored. This action cannot be undone."
        confirmLabel="Reject withdrawal"
        destructive
        loading={isActionPending}
        onConfirm={() => void handleConfirmAction()}
      />

      <ConfirmDialog
        open={confirmAction?.type === "complete"}
        onOpenChange={(open) => !open && setConfirmAction(null)}
        title="Mark withdrawal as completed?"
        description="Confirm that the bank transfer has been sent to the user."
        confirmLabel="Mark completed"
        loading={isActionPending}
        onConfirm={() => void handleConfirmAction()}
      />
    </>
  );
}

interface AdminWithdrawalsListProps {
  withdrawals: AdminWithdrawalListItem[];
  pendingActionId: string | null;
  isActionPending: boolean;
  onViewDetail: (id: string) => void;
  onApprove: (id: string) => void;
  onRequestReject: (id: string) => void;
  onRequestComplete: (id: string) => void;
}

function AdminWithdrawalsDesktopTable({
  withdrawals,
  pendingActionId,
  isActionPending,
  onViewDetail,
  onApprove,
  onRequestReject,
  onRequestComplete,
}: AdminWithdrawalsListProps) {
  return (
    <div className="hidden overflow-hidden rounded-xl border border-border md:block">
      <table className="w-full text-left text-sm">
        <thead className="sticky top-0 z-10 border-b border-border bg-muted/40">
          <tr>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">User</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Amount</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Status</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Requested</th>
            <th scope="col" className="px-4 py-3 text-right text-xs font-medium text-muted-foreground">Actions</th>
          </tr>
        </thead>
        <tbody>
          {withdrawals.map((withdrawal) => (
            <tr
              key={withdrawal.id}
              className="border-b border-border last:border-b-0 hover:bg-muted/30"
            >
              <td className="px-4 py-3">
                <p className="font-medium">{withdrawal.user.fullName}</p>
                <p className="text-xs text-muted-foreground">{withdrawal.user.email}</p>
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
              <td className="px-4 py-3">
                <WithdrawalRowActions
                  withdrawal={withdrawal}
                  pendingActionId={pendingActionId}
                  isActionPending={isActionPending}
                  onViewDetail={onViewDetail}
                  onApprove={onApprove}
                  onRequestReject={onRequestReject}
                  onRequestComplete={onRequestComplete}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function AdminWithdrawalsMobileList(props: AdminWithdrawalsListProps) {
  return (
    <div className="space-y-3 md:hidden">
      {props.withdrawals.map((withdrawal) => (
        <article key={withdrawal.id} className="space-y-3 rounded-xl border border-border p-4">
          <div className="flex items-start justify-between gap-3">
            <div className="min-w-0 space-y-1">
              <p className="text-xl font-semibold tabular-nums">{formatCurrency(withdrawal.amount)}</p>
              <p className="truncate font-medium">{withdrawal.user.fullName}</p>
              <p className="truncate text-xs text-muted-foreground">{withdrawal.user.email}</p>
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
          <p className="text-xs text-muted-foreground">
            Requested {formatDateTime(withdrawal.requestedAt)}
          </p>
          <WithdrawalRowActions {...props} withdrawal={withdrawal} align="start" />
        </article>
      ))}
    </div>
  );
}

function WithdrawalStatusCell({ withdrawal }: { withdrawal: AdminWithdrawalListItem }) {
  return (
    <div className="space-y-1">
      <WithdrawalStatusBadge status={withdrawal.status} label={withdrawal.statusName} />
      <p className="text-xs text-muted-foreground">
        {getWithdrawalStatusDescription(withdrawal.status)}
      </p>
    </div>
  );
}

function WithdrawalRowActions({
  withdrawal,
  pendingActionId,
  isActionPending,
  onViewDetail,
  onApprove,
  onRequestReject,
  onRequestComplete,
  align = "end",
}: {
  withdrawal: AdminWithdrawalListItem;
  pendingActionId: string | null;
  isActionPending: boolean;
  onViewDetail: (id: string) => void;
  onApprove: (id: string) => void;
  onRequestReject: (id: string) => void;
  onRequestComplete: (id: string) => void;
  align?: "end" | "start";
}) {
  const isRowPending = isActionPending && pendingActionId === withdrawal.id;
  const menuActions = [
    { label: "View details", onClick: () => onViewDetail(withdrawal.id) },
    ...(withdrawal.status === WithdrawalStatus.Pending
      ? [{ label: "Reject", onClick: () => onRequestReject(withdrawal.id), destructive: true }]
      : []),
  ];

  return (
    <div className={`flex flex-wrap gap-2 ${align === "end" ? "justify-end" : "justify-start"}`}>
      {withdrawal.status === WithdrawalStatus.Pending ? (
        <Button
          type="button"
          variant="default"
          size="sm"
          disabled={isRowPending}
          onClick={() => void onApprove(withdrawal.id)}
        >
          <CheckCircle2 className="size-4" />
          Approve
        </Button>
      ) : null}

      {withdrawal.status === WithdrawalStatus.Approved ? (
        <Button
          type="button"
          variant="default"
          size="sm"
          disabled={isRowPending}
          onClick={() => onRequestComplete(withdrawal.id)}
        >
          <CheckCircle2 className="size-4" />
          Complete
        </Button>
      ) : null}

      <RowActionsMenu actions={menuActions} disabled={isRowPending} />
    </div>
  );
}

function AdminWithdrawalsTableSkeleton() {
  return (
    <>
      <div className="hidden overflow-hidden rounded-xl border border-border md:block">
        <div className="border-b border-border bg-muted/40 px-4 py-3">
          <Skeleton className="h-3 w-full max-w-lg" />
        </div>
        {Array.from({ length: 5 }).map((_, index) => (
          <div key={index} className="flex gap-4 border-b border-border px-4 py-3 last:border-b-0">
            <Skeleton className="h-4 w-32" />
            <Skeleton className="h-5 w-24" />
            <Skeleton className="h-4 w-20" />
            <Skeleton className="h-4 w-28" />
            <Skeleton className="ml-auto h-8 w-24" />
          </div>
        ))}
      </div>
      <div className="space-y-3 md:hidden">
        {Array.from({ length: 3 }).map((_, index) => (
          <Skeleton key={index} className="h-40 w-full rounded-xl" />
        ))}
      </div>
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
