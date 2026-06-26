"use client";

import { Input } from "@/components/ui/input";

interface AdminOrderDateRangeFilterProps {
  fromDate: string;
  toDate: string;
  onFromDateChange: (value: string) => void;
  onToDateChange: (value: string) => void;
  disabled?: boolean;
}

export function AdminOrderDateRangeFilter({
  fromDate,
  toDate,
  onFromDateChange,
  onToDateChange,
  disabled = false,
}: AdminOrderDateRangeFilterProps) {
  return (
    <div className="grid gap-3 sm:grid-cols-2">
      <div className="space-y-1.5">
        <label htmlFor="admin-order-from-date" className="text-sm font-medium">
          From date
        </label>
        <Input
          id="admin-order-from-date"
          type="date"
          value={fromDate}
          disabled={disabled}
          onChange={(event) => onFromDateChange(event.target.value)}
        />
      </div>

      <div className="space-y-1.5">
        <label htmlFor="admin-order-to-date" className="text-sm font-medium">
          To date
        </label>
        <Input
          id="admin-order-to-date"
          type="date"
          value={toDate}
          disabled={disabled}
          onChange={(event) => onToDateChange(event.target.value)}
        />
      </div>
    </div>
  );
}
