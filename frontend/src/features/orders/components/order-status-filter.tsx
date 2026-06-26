"use client";

import { Button } from "@/components/ui/button";
import { getOrderStatusLabel } from "@/features/orders/lib/order-formatters";
import { OrderStatus, type OrderStatusFilter } from "@/types/order";

const filterOptions: OrderStatusFilter[] = [
  "all",
  OrderStatus.Pending,
  OrderStatus.Approved,
  OrderStatus.Rejected,
];

interface OrderStatusFilterProps {
  value: OrderStatusFilter;
  onChange: (value: OrderStatusFilter) => void;
  disabled?: boolean;
}

export function OrderStatusFilter({
  value,
  onChange,
  disabled = false,
}: OrderStatusFilterProps) {
  return (
    <div className="flex flex-wrap gap-2">
      {filterOptions.map((option) => {
        const isActive = value === option;

        return (
          <Button
            key={option}
            type="button"
            size="sm"
            variant={isActive ? "default" : "outline"}
            disabled={disabled}
            onClick={() => onChange(option)}
          >
            {getOrderStatusLabel(option)}
          </Button>
        );
      })}
    </div>
  );
}
