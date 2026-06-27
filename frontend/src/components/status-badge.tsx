import { cn } from "@/lib/utils";
import { OrderStatus } from "@/types/order";
import { WithdrawalStatus } from "@/types/withdrawal";

type StatusBadgeVariant = "pending" | "success" | "info" | "destructive" | "muted";

interface StatusBadgeProps {
  label: string;
  variant: StatusBadgeVariant;
  className?: string;
}

const variantClasses: Record<StatusBadgeVariant, string> = {
  pending: "bg-warning-muted text-warning",
  success: "bg-success-muted text-success",
  info: "bg-info-muted text-info",
  destructive: "bg-destructive-muted text-destructive",
  muted: "bg-muted text-muted-foreground",
};

export function StatusBadge({ label, variant, className }: StatusBadgeProps) {
  return (
    <span
      className={cn(
        "inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium",
        variantClasses[variant],
        className,
      )}
    >
      {label}
    </span>
  );
}

export function getOrderStatusVariant(status: OrderStatus): StatusBadgeVariant {
  switch (status) {
    case OrderStatus.Pending:
      return "pending";
    case OrderStatus.Approved:
      return "success";
    case OrderStatus.Rejected:
      return "destructive";
    default:
      return "muted";
  }
}

export function OrderStatusBadge({
  status,
  label,
  className,
}: {
  status: OrderStatus;
  label: string;
  className?: string;
}) {
  return (
    <StatusBadge
      label={label}
      variant={getOrderStatusVariant(status)}
      className={className}
    />
  );
}

export function getWithdrawalStatusVariant(status: WithdrawalStatus): StatusBadgeVariant {
  switch (status) {
    case WithdrawalStatus.Pending:
      return "pending";
    case WithdrawalStatus.Approved:
      return "info";
    case WithdrawalStatus.Completed:
      return "success";
    case WithdrawalStatus.Rejected:
      return "destructive";
    default:
      return "muted";
  }
}

export function WithdrawalStatusBadge({
  status,
  label,
  className,
}: {
  status: WithdrawalStatus;
  label: string;
  className?: string;
}) {
  return (
    <StatusBadge
      label={label}
      variant={getWithdrawalStatusVariant(status)}
      className={className}
    />
  );
}
