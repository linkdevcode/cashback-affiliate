"use client";

import { ChevronDown } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import { AdminWithdrawalSearchFilter } from "@/features/admin/withdrawals/components/admin-withdrawal-search-filter";
import { WithdrawalStatusFilter } from "@/features/withdrawals/components/withdrawal-status-filter";
import { getWithdrawalStatusLabel } from "@/features/withdrawals/lib/withdrawal-formatters";
import { cn } from "@/lib/utils";
import type { WithdrawalStatusFilter as WithdrawalStatusFilterValue } from "@/types/withdrawal";

interface AdminWithdrawalFiltersProps {
  user: string;
  statusFilter: WithdrawalStatusFilterValue;
  onUserChange: (value: string) => void;
  onStatusFilterChange: (value: WithdrawalStatusFilterValue) => void;
  onClearFilters: () => void;
  disabled?: boolean;
}

export function AdminWithdrawalFilters({
  user,
  statusFilter,
  onUserChange,
  onStatusFilterChange,
  onClearFilters,
  disabled = false,
}: AdminWithdrawalFiltersProps) {
  const [isExpanded, setIsExpanded] = useState(false);
  const hasActiveFilters = Boolean(user.trim()) || statusFilter !== "all";

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
        <AdminWithdrawalSearchFilter
          user={user}
          onUserChange={onUserChange}
          disabled={disabled}
        />

        <div className="space-y-2">
          <p className="text-xs font-medium text-muted-foreground">Filter by status</p>
          <WithdrawalStatusFilter
            value={statusFilter}
            onChange={onStatusFilterChange}
            disabled={disabled}
          />
        </div>

        {hasActiveFilters ? (
          <div className="flex items-center justify-between border-t border-border pt-4">
            <p className="text-xs text-muted-foreground">
              {statusFilter !== "all" ? `Status: ${getWithdrawalStatusLabel(statusFilter)}` : "Filters active"}
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
