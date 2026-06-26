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
import { OrderStatusTimeline } from "@/features/orders/components/order-status-timeline";
import { useOrderDetail } from "@/features/orders/hooks/use-order-detail";
import {
  formatCurrency,
  formatDateTime,
  getOrderStatusBadgeClass,
} from "@/features/orders/lib/order-formatters";
import { isApiError } from "@/services/api-client";

interface OrderDetailModalProps {
  orderId: string | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function OrderDetailModal({
  orderId,
  open,
  onOpenChange,
}: OrderDetailModalProps) {
  const { data, isLoading, isError, error } = useOrderDetail(orderId);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogHeader>
        <DialogTitle>Order details</DialogTitle>
        <DialogDescription>
          Full information and status timeline for this order.
        </DialogDescription>
      </DialogHeader>

      <DialogContent className="space-y-4">
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
            <div className="grid gap-4 sm:grid-cols-2">
              <DetailField label="Order ID" value={data.orderCode} />
              <DetailField label="Merchant" value={data.merchant ?? "—"} />
              <DetailField
                label="Order amount"
                value={data.orderAmount != null ? formatCurrency(data.orderAmount) : "—"}
              />
              <DetailField label="Created at" value={formatDateTime(data.createdAt)} />
            </div>

            <div className="grid gap-4 sm:grid-cols-2">
              <DetailField label="Commission amount" value={formatCurrency(data.commissionAmount)} />
              <DetailField label="Cashback amount" value={formatCurrency(data.cashbackAmount)} />
            </div>

            <div className="space-y-1.5">
              <p className="text-xs font-medium text-muted-foreground">Status</p>
              <span
                className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getOrderStatusBadgeClass(data.status)}`}
              >
                {data.statusName}
              </span>
            </div>

            <OrderStatusTimeline status={data.status} statusName={data.statusName} />
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
