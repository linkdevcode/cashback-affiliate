"use client";

import { ChevronDown } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import { AdminOrderDateRangeFilter } from "@/features/admin/orders/components/admin-order-date-range-filter";
import { AdminOrderSearchFilters } from "@/features/admin/orders/components/admin-order-search-filters";
import { AdminOrderStatusFilter } from "@/features/admin/orders/components/admin-order-status-filter";
import { getOrderStatusLabel } from "@/features/orders/lib/order-formatters";
import { cn } from "@/lib/utils";
import type { OrderStatusFilter } from "@/types/order";

interface AdminOrderFiltersProps {
  orderId: string;
  user: string;
  fromDate: string;
  toDate: string;
  statusFilter: OrderStatusFilter;
  onOrderIdChange: (value: string) => void;
  onUserChange: (value: string) => void;
  onFromDateChange: (value: string) => void;
  onToDateChange: (value: string) => void;
  onStatusFilterChange: (value: OrderStatusFilter) => void;
  onClearFilters: () => void;
  disabled?: boolean;
}

export function AdminOrderFilters({
  orderId,
  user,
  fromDate,
  toDate,
  statusFilter,
  onOrderIdChange,
  onUserChange,
  onFromDateChange,
  onToDateChange,
  onStatusFilterChange,
  onClearFilters,
  disabled = false,
}: AdminOrderFiltersProps) {
  const [isExpanded, setIsExpanded] = useState(false);
  const hasActiveFilters =
    Boolean(orderId.trim() || user.trim() || fromDate || toDate) ||
    statusFilter !== "all";

  return (
    <div className="space-y-3">
      <div className="flex items-center justify-between gap-3 md:hidden">
        <Button
          type="button"
          variant="outline"
          size="sm"
          className="flex-1 justify-between"
          onClick={() => setIsExpanded((current) => !current)}
          aria-expanded={isExpanded}
        >
          <span>Filters</span>
          <ChevronDown className={cn("size-4 transition-transform", isExpanded && "rotate-180")} />
        </Button>
        {hasActiveFilters ? (
          <Button type="button" variant="ghost" size="sm" onClick={onClearFilters} disabled={disabled}>
            Clear
          </Button>
        ) : null}
      </div>

      <div
        className={cn(
          "space-y-4 rounded-lg bg-muted/30 p-4",
          !isExpanded && "hidden md:block",
          isExpanded && "block",
        )}
      >
        <AdminOrderSearchFilters
          orderId={orderId}
          user={user}
          onOrderIdChange={onOrderIdChange}
          onUserChange={onUserChange}
          disabled={disabled}
        />

        <AdminOrderDateRangeFilter
          fromDate={fromDate}
          toDate={toDate}
          onFromDateChange={onFromDateChange}
          onToDateChange={onToDateChange}
          disabled={disabled}
        />

        <div className="space-y-2">
          <p className="text-xs font-medium text-muted-foreground">Filter by status</p>
          <AdminOrderStatusFilter
            value={statusFilter}
            onChange={onStatusFilterChange}
            disabled={disabled}
          />
        </div>

        {hasActiveFilters ? (
          <div className="flex items-center justify-between border-t border-border pt-4">
            <p className="text-xs text-muted-foreground">
              {statusFilter !== "all" ? `Status: ${getOrderStatusLabel(statusFilter)}` : "Filters active"}
            </p>
            <Button type="button" variant="ghost" size="sm" onClick={onClearFilters} disabled={disabled}>
              Clear filters
            </Button>
          </div>
        ) : null}
      </div>
    </div>
  );
}
