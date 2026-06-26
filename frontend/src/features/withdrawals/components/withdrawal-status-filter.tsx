"use client";

import { Button } from "@/components/ui/button";
import { getWithdrawalStatusLabel } from "@/features/withdrawals/lib/withdrawal-formatters";
import { WithdrawalStatus, type WithdrawalStatusFilter } from "@/types/withdrawal";

const FILTER_OPTIONS: WithdrawalStatusFilter[] = [
  "all",
  WithdrawalStatus.Pending,
  WithdrawalStatus.Approved,
  WithdrawalStatus.Rejected,
  WithdrawalStatus.Completed,
];

interface WithdrawalStatusFilterProps {
  value: WithdrawalStatusFilter;
  onChange: (value: WithdrawalStatusFilter) => void;
  disabled?: boolean;
}

export function WithdrawalStatusFilter({
  value,
  onChange,
  disabled = false,
}: WithdrawalStatusFilterProps) {
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
          {getWithdrawalStatusLabel(option)}
        </Button>
      ))}
    </div>
  );
}
