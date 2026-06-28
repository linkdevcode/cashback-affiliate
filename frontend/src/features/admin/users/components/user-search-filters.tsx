"use client";

import { Input } from "@/components/ui/input";

interface UserSearchFiltersProps {
  email: string;
  name: string;
  onEmailChange: (value: string) => void;
  onNameChange: (value: string) => void;
  disabled?: boolean;
}

export function UserSearchFilters({
  email,
  name,
  onEmailChange,
  onNameChange,
  disabled = false,
}: UserSearchFiltersProps) {
  return (
    <div className="grid gap-3 sm:grid-cols-2">
      <div className="space-y-1.5">
        <label htmlFor="user-email-search" className="text-sm font-medium">
          Search by email
        </label>
        <Input
          id="user-email-search"
          type="search"
          placeholder="user@example.com"
          value={email}
          disabled={disabled}
          onChange={(event) => onEmailChange(event.target.value)}
        />
      </div>

      <div className="space-y-1.5">
        <label htmlFor="user-name-search" className="text-sm font-medium">
          Search by name
        </label>
        <Input
          id="user-name-search"
          type="search"
          placeholder="John Doe"
          value={name}
          disabled={disabled}
          onChange={(event) => onNameChange(event.target.value)}
        />
      </div>
    </div>
  );
}
