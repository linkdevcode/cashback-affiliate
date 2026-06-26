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
      return "bg-amber-100 text-amber-800 dark:bg-amber-950 dark:text-amber-300";
    case OrderStatus.Approved:
      return "bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300";
    case OrderStatus.Rejected:
      return "bg-rose-100 text-rose-800 dark:bg-rose-950 dark:text-rose-300";
    default:
      return "bg-muted text-muted-foreground";
  }
}
