"use client";

import { Input } from "@/components/ui/input";

interface AdminOrderSearchFiltersProps {
  orderId: string;
  user: string;
  onOrderIdChange: (value: string) => void;
  onUserChange: (value: string) => void;
  disabled?: boolean;
}

export function AdminOrderSearchFilters({
  orderId,
  user,
  onOrderIdChange,
  onUserChange,
  disabled = false,
}: AdminOrderSearchFiltersProps) {
  return (
    <div className="grid gap-3 sm:grid-cols-2">
      <div className="space-y-1.5">
        <label htmlFor="admin-order-id-search" className="text-sm font-medium">
          Search by order ID
        </label>
        <Input
          id="admin-order-id-search"
          type="search"
          placeholder="ORDER-12345"
          value={orderId}
          disabled={disabled}
          onChange={(event) => onOrderIdChange(event.target.value)}
        />
      </div>

      <div className="space-y-1.5">
        <label htmlFor="admin-order-user-search" className="text-sm font-medium">
          Search by user
        </label>
        <Input
          id="admin-order-user-search"
          type="search"
          placeholder="Email or name"
          value={user}
          disabled={disabled}
          onChange={(event) => onUserChange(event.target.value)}
        />
      </div>
    </div>
  );
}
