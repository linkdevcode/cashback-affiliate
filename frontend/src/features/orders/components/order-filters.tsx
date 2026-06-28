"use client";

import { ChevronDown, Search, X } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { OrderStatusFilter } from "@/features/orders/components/order-status-filter";
import { getOrderStatusLabel } from "@/features/orders/lib/order-formatters";
import { cn } from "@/lib/utils";
import { OrderStatus, type OrderStatusFilter as OrderStatusFilterValue } from "@/types/order";

interface OrderFiltersProps {
  statusFilter: OrderStatusFilterValue;
  searchQuery: string;
  onStatusFilterChange: (value: OrderStatusFilterValue) => void;
  onSearchQueryChange: (value: string) => void;
  onClearFilters: () => void;
  disabled?: boolean;
  hasActiveFilters?: boolean;
}

export function OrderFilters({
  statusFilter,
  searchQuery,
  onStatusFilterChange,
  onSearchQueryChange,
  onClearFilters,
  disabled = false,
  hasActiveFilters = false,
}: OrderFiltersProps) {
  const [isExpanded, setIsExpanded] = useState(false);

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
          <ChevronDown
            className={cn("size-4 transition-transform", isExpanded && "rotate-180")}
          />
        </Button>
        {hasActiveFilters ? (
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={onClearFilters}
            disabled={disabled}
          >
            Clear
          </Button>
        ) : null}
      </div>

      <div
        className={cn(
          "rounded-lg bg-muted/30 p-4",
          !isExpanded && "hidden md:block",
          isExpanded && "block",
        )}
      >
        <div className="flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
          <div className="w-full space-y-1.5 lg:max-w-xs">
            <label htmlFor="order-search" className="text-xs font-medium text-muted-foreground">
              Search by order ID
            </label>
            <div className="relative">
              <Search
                className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground"
                aria-hidden="true"
              />
              <Input
                id="order-search"
                type="search"
                placeholder="e.g. AT-12345"
                value={searchQuery}
                disabled={disabled}
                className="pl-9"
                onChange={(event) => onSearchQueryChange(event.target.value)}
              />
              {searchQuery ? (
                <button
                  type="button"
                  className="absolute top-1/2 right-2 -translate-y-1/2 rounded-md p-1 text-muted-foreground hover:text-foreground"
                  aria-label="Clear search"
                  onClick={() => onSearchQueryChange("")}
                >
                  <X className="size-4" />
                </button>
              ) : null}
            </div>
            {searchQuery ? (
              <p className="text-xs text-muted-foreground">
                Matching orders on this page
              </p>
            ) : null}
          </div>

          <div className="space-y-2">
            <p className="text-xs font-medium text-muted-foreground">Filter by status</p>
            <OrderStatusFilter
              value={statusFilter}
              onChange={onStatusFilterChange}
              disabled={disabled}
            />
          </div>
        </div>

        {hasActiveFilters ? (
          <div className="mt-4 hidden items-center justify-between border-t border-border pt-4 md:flex">
            <p className="text-xs text-muted-foreground">
              {getActiveFiltersLabel(statusFilter, searchQuery)}
            </p>
            <Button
              type="button"
              variant="ghost"
              size="sm"
              onClick={onClearFilters}
              disabled={disabled}
            >
              Clear filters
            </Button>
          </div>
        ) : null}
      </div>
    </div>
  );
}

function getActiveFiltersLabel(
  statusFilter: OrderStatusFilterValue,
  searchQuery: string,
): string {
  const parts: string[] = [];

  if (searchQuery.trim()) {
    parts.push(`Search: "${searchQuery.trim()}"`);
  }

  if (statusFilter !== "all") {
    parts.push(`Status: ${getOrderStatusLabel(statusFilter)}`);
  }

  return parts.join(" · ");
}

export function hasOrderFiltersActive(
  statusFilter: OrderStatusFilterValue,
  searchQuery: string,
): boolean {
  return statusFilter !== "all" || searchQuery.trim().length > 0;
}

export { OrderStatus };
