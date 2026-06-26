"use client";

import { Loader2, UserCheck, UserX } from "lucide-react";
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
import { useActivateUser } from "@/features/admin/users/hooks/use-activate-user";
import { useAdminUserDetail } from "@/features/admin/users/hooks/use-admin-user-detail";
import { useSuspendUser } from "@/features/admin/users/hooks/use-suspend-user";
import {
  canManageUserStatus,
  formatCurrency,
  formatDateTime,
  getUserRoleLabel,
  getUserStatusBadgeClass,
  getUserStatusLabel,
} from "@/features/admin/users/lib/user-formatters";
import { isApiError } from "@/services/api-client";
import { UserStatusValue } from "@/types/auth";

interface UserDetailModalProps {
  userId: string | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function UserDetailModal({
  userId,
  open,
  onOpenChange,
}: UserDetailModalProps) {
  const { data, isLoading, isError, error } = useAdminUserDetail(userId);
  const suspendUser = useSuspendUser();
  const activateUser = useActivateUser();
  const [actionError, setActionError] = useState<string | null>(null);

  const handleSuspend = async () => {
    if (!userId) {
      return;
    }

    setActionError(null);

    try {
      await suspendUser.mutateAsync(userId);
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    }
  };

  const handleActivate = async () => {
    if (!userId) {
      return;
    }

    setActionError(null);

    try {
      await activateUser.mutateAsync(userId);
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    }
  };

  const isActionPending = suspendUser.isPending || activateUser.isPending;
  const profile = data?.profile;
  const showStatusActions = profile
    ? canManageUserStatus(profile.role, profile.status)
    : false;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogHeader>
        <DialogTitle>User details</DialogTitle>
        <DialogDescription>
          Profile information, order summary, and withdrawal summary.
        </DialogDescription>
      </DialogHeader>

      <DialogContent className="max-h-[80vh] space-y-4 overflow-y-auto">
        {isLoading ? (
          <div className="flex items-center justify-center py-8 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading user details...
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
              <h3 className="text-sm font-semibold">Profile</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField label="Email" value={data.profile.email} />
                <DetailField label="Name" value={data.profile.fullName} />
                <DetailField label="Role" value={getUserRoleLabel(data.profile.role)} />
                <DetailField
                  label="Created at"
                  value={formatDateTime(data.profile.createdAt)}
                />
                <DetailField
                  label="Last login"
                  value={
                    data.profile.lastLoginAt
                      ? formatDateTime(data.profile.lastLoginAt)
                      : "—"
                  }
                />
                <DetailField
                  label="Available balance"
                  value={formatCurrency(data.profile.availableBalance)}
                />
                <DetailField
                  label="Pending balance"
                  value={formatCurrency(data.profile.pendingBalance)}
                />
                <DetailField
                  label="Lifetime cashback"
                  value={formatCurrency(data.profile.lifetimeCashback)}
                />
              </div>

              <div className="space-y-1.5">
                <p className="text-xs font-medium text-muted-foreground">Status</p>
                <span
                  className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getUserStatusBadgeClass(data.profile.status)}`}
                >
                  {getUserStatusLabel(data.profile.status)}
                </span>
              </div>
            </section>

            <section className="space-y-3">
              <h3 className="text-sm font-semibold">Order summary</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField
                  label="Total orders"
                  value={String(data.orderSummary.totalOrders)}
                />
                <DetailField
                  label="Pending orders"
                  value={String(data.orderSummary.pendingOrders)}
                />
                <DetailField
                  label="Approved orders"
                  value={String(data.orderSummary.approvedOrders)}
                />
                <DetailField
                  label="Rejected orders"
                  value={String(data.orderSummary.rejectedOrders)}
                />
                <DetailField
                  label="Total commission"
                  value={formatCurrency(data.orderSummary.totalCommission)}
                />
                <DetailField
                  label="Total cashback"
                  value={formatCurrency(data.orderSummary.totalCashback)}
                />
              </div>
            </section>

            <section className="space-y-3">
              <h3 className="text-sm font-semibold">Withdrawal summary</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField
                  label="Total withdrawals"
                  value={String(data.withdrawalSummary.totalWithdrawals)}
                />
                <DetailField
                  label="Pending withdrawals"
                  value={String(data.withdrawalSummary.pendingWithdrawals)}
                />
                <DetailField
                  label="Completed withdrawals"
                  value={String(data.withdrawalSummary.completedWithdrawals)}
                />
                <DetailField
                  label="Total withdrawn"
                  value={formatCurrency(data.withdrawalSummary.totalWithdrawn)}
                />
              </div>
            </section>
          </>
        ) : null}
      </DialogContent>

      <DialogFooter className="gap-2 sm:justify-between">
        <div className="flex flex-wrap gap-2">
          {showStatusActions && profile?.status === UserStatusValue.Active ? (
            <Button
              type="button"
              variant="destructive"
              size="sm"
              disabled={isActionPending}
              onClick={() => void handleSuspend()}
            >
              <UserX />
              Suspend
            </Button>
          ) : null}

          {showStatusActions && profile?.status === UserStatusValue.Suspended ? (
            <Button
              type="button"
              variant="outline"
              size="sm"
              disabled={isActionPending}
              onClick={() => void handleActivate()}
            >
              <UserCheck />
              Activate
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
    return error.response?.data?.message ?? "Unable to load user details.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load user details.";
}
