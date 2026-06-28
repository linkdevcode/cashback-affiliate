"use client";

import { OrderStatusBadge } from "@/components/status-badge";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Skeleton } from "@/components/ui/skeleton";
import { OrderStatusTimeline } from "@/features/orders/components/order-status-timeline";
import { useOrderDetail } from "@/features/orders/hooks/use-order-detail";
import {
  formatCurrency,
  formatDateTime,
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

      <DialogContent className="space-y-6">
        {isLoading ? <OrderDetailSkeleton /> : null}

        {isError ? (
          <p className="text-sm text-destructive" role="alert">
            {getErrorMessage(error)}
          </p>
        ) : null}

        {data ? (
          <>
            <div className="flex flex-wrap items-center justify-between gap-3">
              <div className="space-y-1">
                <p className="text-xs font-medium text-muted-foreground">Order ID</p>
                <p className="font-mono text-base font-semibold">{data.orderCode}</p>
              </div>
              <OrderStatusBadge status={data.status} label={data.statusName} />
            </div>

            <div className="grid gap-4 sm:grid-cols-2">
              <DetailField label="Merchant" value={data.merchant ?? "—"} />
              <DetailField
                label="Order amount"
                value={data.orderAmount != null ? formatCurrency(data.orderAmount) : "—"}
                tabular
              />
              <DetailField label="Created at" value={formatDateTime(data.createdAt)} />
              <DetailField
                label="Commission amount"
                value={formatCurrency(data.commissionAmount)}
                tabular
              />
              <DetailField
                label="Cashback amount"
                value={formatCurrency(data.cashbackAmount)}
                tabular
                emphasized
              />
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
  tabular?: boolean;
  emphasized?: boolean;
}

function DetailField({ label, value, tabular = false, emphasized = false }: DetailFieldProps) {
  return (
    <div className="space-y-1.5">
      <p className="text-xs font-medium text-muted-foreground">{label}</p>
      <p
        className={`rounded-lg bg-muted/30 px-3 py-2 text-sm ${
          tabular ? "tabular-nums" : ""
        } ${emphasized ? "font-medium" : ""}`}
      >
        {value}
      </p>
    </div>
  );
}

function OrderDetailSkeleton() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between gap-3">
        <div className="space-y-2">
          <Skeleton className="h-3 w-16" />
          <Skeleton className="h-5 w-32" />
        </div>
        <Skeleton className="h-6 w-20 rounded-full" />
      </div>
      <div className="grid gap-4 sm:grid-cols-2">
        {Array.from({ length: 5 }).map((_, index) => (
          <div key={index} className="space-y-2">
            <Skeleton className="h-3 w-20" />
            <Skeleton className="h-10 w-full rounded-lg" />
          </div>
        ))}
      </div>
      <div className="space-y-3">
        <Skeleton className="h-3 w-24" />
        {Array.from({ length: 3 }).map((_, index) => (
          <Skeleton key={index} className="h-8 w-full" />
        ))}
      </div>
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
