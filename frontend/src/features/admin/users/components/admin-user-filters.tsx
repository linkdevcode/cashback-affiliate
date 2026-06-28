"use client";

import { ChevronDown } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import { UserSearchFilters } from "@/features/admin/users/components/user-search-filters";
import { UserStatusFilter } from "@/features/admin/users/components/user-status-filter";
import { getUserStatusFilterLabel } from "@/features/admin/users/lib/user-formatters";
import { cn } from "@/lib/utils";
import type { AdminUserStatusFilter } from "@/types/admin-user";

interface AdminUserFiltersProps {
  email: string;
  name: string;
  statusFilter: AdminUserStatusFilter;
  onEmailChange: (value: string) => void;
  onNameChange: (value: string) => void;
  onStatusFilterChange: (value: AdminUserStatusFilter) => void;
  onClearFilters: () => void;
  disabled?: boolean;
}

export function AdminUserFilters({
  email,
  name,
  statusFilter,
  onEmailChange,
  onNameChange,
  onStatusFilterChange,
  onClearFilters,
  disabled = false,
}: AdminUserFiltersProps) {
  const [isExpanded, setIsExpanded] = useState(false);
  const hasActiveFilters =
    Boolean(email.trim() || name.trim()) || statusFilter !== "all";

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
        <UserSearchFilters
          email={email}
          name={name}
          onEmailChange={onEmailChange}
          onNameChange={onNameChange}
          disabled={disabled}
        />

        <div className="space-y-2">
          <p className="text-xs font-medium text-muted-foreground">Filter by status</p>
          <UserStatusFilter
            value={statusFilter}
            onChange={onStatusFilterChange}
            disabled={disabled}
          />
        </div>

        {hasActiveFilters ? (
          <div className="flex items-center justify-between border-t border-border pt-4">
            <p className="text-xs text-muted-foreground">
              {statusFilter !== "all"
                ? `Status: ${getUserStatusFilterLabel(statusFilter)}`
                : "Filters active"}
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
