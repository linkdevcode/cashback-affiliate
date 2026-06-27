import { OrderStatus, type OrderStatusFilter } from "@/types/order";

export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
    maximumFractionDigits: 0,
  }).format(amount);
}

export function formatDateTime(value: string): string {
  return new Intl.DateTimeFormat("vi-VN", {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(value));
}

export function getOrderStatusLabel(status: OrderStatusFilter): string {
  switch (status) {
    case "all":
      return "All";
    case OrderStatus.Pending:
      return "Pending";
    case OrderStatus.Approved:
      return "Approved";
    case OrderStatus.Rejected:
      return "Rejected";
    default:
      return "All";
  }
}

export function getOrderStatusBadgeClass(status: OrderStatus): string {
  switch (status) {
    case OrderStatus.Pending:
      return "bg-warning-muted text-warning";
    case OrderStatus.Approved:
      return "bg-success-muted text-success";
    case OrderStatus.Rejected:
      return "bg-destructive-muted text-destructive";
    default:
      return "bg-muted text-muted-foreground";
  }
}
