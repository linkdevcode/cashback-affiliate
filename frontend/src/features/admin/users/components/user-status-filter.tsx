"use client";

import { Button } from "@/components/ui/button";
import { getUserStatusFilterLabel } from "@/features/admin/users/lib/user-formatters";
import { UserStatusValue } from "@/types/auth";
import type { AdminUserStatusFilter } from "@/types/admin-user";

const FILTER_OPTIONS: AdminUserStatusFilter[] = [
  "all",
  UserStatusValue.Active,
  UserStatusValue.Suspended,
];

interface UserStatusFilterProps {
  value: AdminUserStatusFilter;
  onChange: (value: AdminUserStatusFilter) => void;
  disabled?: boolean;
}

export function UserStatusFilter({
  value,
  onChange,
  disabled = false,
}: UserStatusFilterProps) {
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
          {getUserStatusFilterLabel(option)}
        </Button>
      ))}
    </div>
  );
}
