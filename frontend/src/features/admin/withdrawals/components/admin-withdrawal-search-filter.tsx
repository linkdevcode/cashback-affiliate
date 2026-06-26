"use client";

import { Input } from "@/components/ui/input";

interface AdminWithdrawalSearchFilterProps {
  user: string;
  onUserChange: (value: string) => void;
  disabled?: boolean;
}

export function AdminWithdrawalSearchFilter({
  user,
  onUserChange,
  disabled = false,
}: AdminWithdrawalSearchFilterProps) {
  return (
    <div className="space-y-1.5">
      <label htmlFor="admin-withdrawal-user-search" className="text-sm font-medium">
        Search by user
      </label>
      <Input
        id="admin-withdrawal-user-search"
        type="search"
        placeholder="Email or name"
        value={user}
        disabled={disabled}
        onChange={(event) => onUserChange(event.target.value)}
      />
    </div>
  );
}
