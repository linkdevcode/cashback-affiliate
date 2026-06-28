"use client";

import { CheckCircle2, Loader2, XCircle } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { useAdminWithdrawalDetail } from "@/features/admin/withdrawals/hooks/use-admin-withdrawal-detail";
import { useApproveWithdrawal } from "@/features/admin/withdrawals/hooks/use-approve-withdrawal";
import { useCompleteWithdrawal } from "@/features/admin/withdrawals/hooks/use-complete-withdrawal";
import { useRejectWithdrawal } from "@/features/admin/withdrawals/hooks/use-reject-withdrawal";
import {
  formatCurrency,
  formatDateTime,
} from "@/features/orders/lib/order-formatters";
import { getWithdrawalStatusBadgeClass } from "@/features/withdrawals/lib/withdrawal-formatters";
import { isApiError } from "@/services/api-client";
import { WithdrawalStatus } from "@/types/withdrawal";

interface AdminWithdrawalDetailModalProps {
  withdrawalId: string | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AdminWithdrawalDetailModal({
  withdrawalId,
  open,
  onOpenChange,
}: AdminWithdrawalDetailModalProps) {
  const { data, isLoading, isError, error } = useAdminWithdrawalDetail(withdrawalId);
  const approveWithdrawal = useApproveWithdrawal();
  const rejectWithdrawal = useRejectWithdrawal();
  const completeWithdrawal = useCompleteWithdrawal();
  const [actionError, setActionError] = useState<string | null>(null);
  const [rejectReason, setRejectReason] = useState("");
  const [showRejectForm, setShowRejectForm] = useState(false);

  const handleApprove = async () => {
    if (!withdrawalId) {
      return;
    }

    setActionError(null);

    try {
      await approveWithdrawal.mutateAsync(withdrawalId);
      setShowRejectForm(false);
      setRejectReason("");
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    }
  };

  const handleReject = async () => {
    if (!withdrawalId) {
      return;
    }

    setActionError(null);

    try {
      await rejectWithdrawal.mutateAsync({
        withdrawalId,
        request: rejectReason.trim() ? { reason: rejectReason.trim() } : undefined,
      });
      setShowRejectForm(false);
      setRejectReason("");
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    }
  };

  const handleComplete = async () => {
    if (!withdrawalId) {
      return;
    }

    setActionError(null);

    try {
      await completeWithdrawal.mutateAsync(withdrawalId);
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    }
  };

  const isActionPending =
    approveWithdrawal.isPending ||
    rejectWithdrawal.isPending ||
    completeWithdrawal.isPending;

  const canApprove = data?.status === WithdrawalStatus.Pending;
  const canReject = data?.status === WithdrawalStatus.Pending;
  const canComplete = data?.status === WithdrawalStatus.Approved;

  return (
    <Dialog
      open={open}
      onOpenChange={(nextOpen) => {
        if (!nextOpen) {
          setShowRejectForm(false);
          setRejectReason("");
          setActionError(null);
        }

        onOpenChange(nextOpen);
      }}
    >
      <DialogHeader>
        <DialogTitle>Withdrawal details</DialogTitle>
        <DialogDescription>
          Review bank information and process the withdrawal request.
        </DialogDescription>
      </DialogHeader>

      <DialogContent className="max-h-[80vh] space-y-4 overflow-y-auto">
        {isLoading ? (
          <div className="flex items-center justify-center py-8 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading withdrawal details...
          </div>
        ) : null}

        {isError ? (
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        ) : null}

        {actionError ? (
          <p className="text-sm text-destructive" role="alert">
            {actionError}
          </p>
        ) : null}

        {data ? (
          <>
            <section className="space-y-3">
              <h3 className="text-sm font-semibold">User</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField label="Name" value={data.user.fullName} />
                <DetailField label="Email" value={data.user.email} />
              </div>
            </section>

            <section className="space-y-3">
              <h3 className="text-sm font-semibold">Withdrawal</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField label="Amount" value={formatCurrency(data.amount)} />
                <DetailField
                  label="Requested at"
                  value={formatDateTime(data.requestedAt)}
                />
                <DetailField
                  label="Processed at"
                  value={data.processedAt ? formatDateTime(data.processedAt) : "—"}
                />
              </div>

              <div className="space-y-1.5">
                <p className="text-xs font-medium text-muted-foreground">Status</p>
                <span
                  className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getWithdrawalStatusBadgeClass(data.status)}`}
                >
                  {data.statusName}
                </span>
              </div>
            </section>

            <section className="space-y-3">
              <h3 className="text-sm font-semibold">Bank information</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField label="Bank name" value={data.bankName} />
                <DetailField label="Account holder" value={data.bankAccountHolder} />
                <DetailField
                  label="Account number"
                  value={data.bankAccountNumber}
                />
              </div>
            </section>

            {showRejectForm ? (
              <section className="space-y-2">
                <label htmlFor="reject-reason" className="text-sm font-medium">
                  Rejection reason (optional)
                </label>
                <Input
                  id="reject-reason"
                  type="text"
                  placeholder="Invalid bank account"
                  value={rejectReason}
                  disabled={isActionPending}
                  onChange={(event) => setRejectReason(event.target.value)}
                />
              </section>
            ) : null}
          </>
        ) : null}
      </DialogContent>

      <DialogFooter className="gap-2 sm:justify-between">
        <div className="flex flex-wrap gap-2">
          {canApprove ? (
            <Button
              type="button"
              size="sm"
              disabled={isActionPending}
              onClick={() => void handleApprove()}
            >
              <CheckCircle2 />
              Approve
            </Button>
          ) : null}

          {canReject && !showRejectForm ? (
            <Button
              type="button"
              variant="destructive"
              size="sm"
              disabled={isActionPending}
              onClick={() => setShowRejectForm(true)}
            >
              <XCircle />
              Reject
            </Button>
          ) : null}

          {canReject && showRejectForm ? (
            <>
              <Button
                type="button"
                variant="destructive"
                size="sm"
                disabled={isActionPending}
                onClick={() => void handleReject()}
              >
                Confirm reject
              </Button>
              <Button
                type="button"
                variant="outline"
                size="sm"
                disabled={isActionPending}
                onClick={() => {
                  setShowRejectForm(false);
                  setRejectReason("");
                }}
              >
                Cancel
              </Button>
            </>
          ) : null}

          {canComplete ? (
            <Button
              type="button"
              size="sm"
              disabled={isActionPending}
              onClick={() => void handleComplete()}
            >
              <CheckCircle2 />
              Complete
            </Button>
          ) : null}
        </div>

        <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
          Close
        </Button>
      </DialogFooter>
    </Dialog>
  );
}

interface DetailFieldProps {
  label: string;
  value: string;
}

function DetailField({ label, value }: DetailFieldProps) {
  return (
    <div className="space-y-1.5">
      <p className="text-xs font-medium text-muted-foreground">{label}</p>
      <p className="rounded-lg border bg-muted/30 px-3 py-2 text-sm">{value}</p>
    </div>
  );
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to load withdrawal details.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load withdrawal details.";
}
