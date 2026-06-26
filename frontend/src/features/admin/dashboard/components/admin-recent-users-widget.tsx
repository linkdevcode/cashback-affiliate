"use client";

import Link from "next/link";
import { Loader2 } from "lucide-react";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  formatDateTime,
  getUserStatusBadgeClass,
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
        <div>
          <CardTitle>Recent users</CardTitle>
          <CardDescription>Latest registered accounts</CardDescription>
        </div>
        <Link
          href="/admin/users"
          className="text-sm font-medium text-primary hover:underline"
        >
          View all
        </Link>
      </CardHeader>
      <CardContent>
        {isLoading ? (
          <div className="flex items-center justify-center py-10 text-sm text-muted-foreground">
            <Loader2 className="mr-2 size-4 animate-spin" />
            Loading recent users...
          </div>
        ) : null}

        {!isLoading && users.length === 0 ? (
          <p className="py-10 text-center text-sm text-muted-foreground">
            No recent users yet.
          </p>
        ) : null}

        {!isLoading && users.length > 0 ? (
          <div className="space-y-3">
            {users.map((user) => (
              <div
                key={user.id}
                className="flex items-start justify-between gap-3 rounded-lg border bg-muted/20 px-3 py-2"
              >
                <div className="min-w-0">
                  <p className="truncate font-medium">{user.fullName}</p>
                  <p className="truncate text-xs text-muted-foreground">
                    {user.email}
                  </p>
                  <p className="mt-1 text-xs text-muted-foreground">
                    {formatDateTime(user.createdAt)}
                  </p>
                </div>
                <span
                  className={`shrink-0 rounded-full px-2 py-0.5 text-xs font-medium ${getUserStatusBadgeClass(user.status)}`}
                >
                  {getUserStatusLabel(user.status)}
                </span>
              </div>
            ))}
          </div>
        ) : null}
      </CardContent>
    </Card>
  );
}
