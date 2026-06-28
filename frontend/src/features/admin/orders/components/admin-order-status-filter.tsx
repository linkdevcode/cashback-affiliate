"use client";

import { Button } from "@/components/ui/button";
import { OrderStatus, type OrderStatusFilter } from "@/types/order";

const FILTER_OPTIONS: OrderStatusFilter[] = [
  "all",
  OrderStatus.Pending,
  OrderStatus.Approved,
  OrderStatus.Rejected,
];

interface AdminOrderStatusFilterProps {
  value: OrderStatusFilter;
  onChange: (value: OrderStatusFilter) => void;
  disabled?: boolean;
}

export function AdminOrderStatusFilter({
  value,
  onChange,
  disabled = false,
}: AdminOrderStatusFilterProps) {
  return (
    <div className="flex flex-wrap gap-2">
      {FILTER_OPTIONS.map((option) => (
        <Button
          key={option}
          type="button"
          size="sm"
          variant={value === option ? "default" : "outline"}
          disabled={disabled}
          onClick={() => onChange(option)}
        >
          {getFilterLabel(option)}
        </Button>
      ))}
    </div>
  );
}

function getFilterLabel(option: OrderStatusFilter): string {
  switch (option) {
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
