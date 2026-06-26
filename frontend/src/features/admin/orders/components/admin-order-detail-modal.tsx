"use client";

import { Loader2 } from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { useAdminOrderDetail } from "@/features/admin/orders/hooks/use-admin-order-detail";
import {
  formatCurrency,
  formatDateTime,
  getOrderStatusBadgeClass,
} from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";

interface AdminOrderDetailModalProps {
  orderId: string | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AdminOrderDetailModal({
  orderId,
  open,
  onOpenChange,
}: AdminOrderDetailModalProps) {
  const { data, isLoading, isError, error } = useAdminOrderDetail(orderId);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogHeader>
        <DialogTitle>Order details</DialogTitle>
        <DialogDescription>
          Full order information and associated user details.
        </DialogDescription>
      </DialogHeader>

      <DialogContent className="max-h-[80vh] space-y-4 overflow-y-auto">
        {isLoading ? (
          <div className="flex items-center justify-center py-8 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading order details...
          </div>
        ) : null}

        {isError ? (
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        ) : null}

        {data ? (
          <>
            <section className="space-y-3">
              <h3 className="text-sm font-semibold">Order</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField label="Order ID" value={data.orderCode} />
                <DetailField label="Merchant" value={data.merchant ?? "—"} />
                <DetailField
                  label="Order amount"
                  value={
                    data.orderAmount != null
                      ? formatCurrency(data.orderAmount)
                      : "—"
                  }
                />
                <DetailField
                  label="Created at"
                  value={formatDateTime(data.createdAt)}
                />
                <DetailField
                  label="Commission"
                  value={formatCurrency(data.commissionAmount)}
                />
                <DetailField
                  label="Cashback"
                  value={formatCurrency(data.cashbackAmount)}
                />
                <DetailField
                  label="Platform profit"
                  value={formatCurrency(data.platformProfit)}
                />
              </div>

              <div className="space-y-1.5">
                <p className="text-xs font-medium text-muted-foreground">Status</p>
                <span
                  className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getOrderStatusBadgeClass(data.status)}`}
                >
                  {data.statusName}
                </span>
              </div>
            </section>

            <section className="space-y-3">
              <h3 className="text-sm font-semibold">User</h3>
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField label="Name" value={data.user.fullName} />
                <DetailField label="Email" value={data.user.email} />
              </div>
            </section>
          </>
        ) : null}
      </DialogContent>

      <DialogFooter>
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
    return error.response?.data?.message ?? "Unable to load order details.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to load order details.";
}
