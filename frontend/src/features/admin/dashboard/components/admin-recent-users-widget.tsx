"use client";

import Link from "next/link";
import { Users } from "lucide-react";

import { EmptyState } from "@/components/empty-state";
import { UserStatusBadge } from "@/components/status-badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import {
  formatDateTime,
  getUserStatusLabel,
} from "@/features/admin/users/lib/user-formatters";
import type { AdminRecentUser } from "@/types/admin-dashboard";

interface AdminRecentUsersWidgetProps {
  users: AdminRecentUser[];
  isLoading?: boolean;
}

export function AdminRecentUsersWidget({
  users,
  isLoading = false,
}: AdminRecentUsersWidgetProps) {
  return (
    <Card className="h-full">
      <CardHeader className="flex flex-col gap-2 sm:flex-row sm:items-start sm:justify-between">
        <div className="space-y-1">
          <CardTitle className="text-lg font-semibold">Recent users</CardTitle>
          <CardDescription>Latest registered accounts</CardDescription>
        </div>
        <Link href="/admin/users" className="text-sm font-medium text-brand hover:underline">
          View all
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? <RecentUsersSkeleton /> : null}

        {!isLoading && users.length === 0 ? (
          <EmptyState
            icon={Users}
            title="No recent users"
            description="New user registrations will appear here."
          />
        ) : null}

        {!isLoading && users.length > 0 ? (
          <div className="space-y-3">
            {users.map((user) => (
              <div
                key={user.id}
                className="flex items-start justify-between gap-3 rounded-lg bg-muted/30 px-3 py-2"
              >
                <div className="min-w-0">
                  <p className="truncate font-medium">{user.fullName}</p>
                  <p className="truncate text-xs text-muted-foreground">{user.email}</p>
                  <p className="mt-1 text-xs text-muted-foreground">
                    {formatDateTime(user.createdAt)}
                  </p>
                </div>
                <UserStatusBadge
                  status={user.status}
                  label={getUserStatusLabel(user.status)}
                  className="shrink-0"
                />
              </div>
            ))}
          </div>
        ) : null}
      </CardContent>
    </Card>
  );
}

function RecentUsersSkeleton() {
  return (
    <div className="space-y-3">
      {Array.from({ length: 4 }).map((_, index) => (
        <Skeleton key={index} className="h-16 w-full rounded-lg" />
      ))}
    </div>
  );
}
