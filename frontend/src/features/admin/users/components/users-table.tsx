"use client";

import { useEffect, useState } from "react";
import { Eye, Loader2, UserCheck, UserX } from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { UserDetailModal } from "@/features/admin/users/components/user-detail-modal";
import { UserSearchFilters } from "@/features/admin/users/components/user-search-filters";
import { UserStatusFilter } from "@/features/admin/users/components/user-status-filter";
import { UsersPagination } from "@/features/admin/users/components/users-pagination";
import { useActivateUser } from "@/features/admin/users/hooks/use-activate-user";
import { useAdminUsers } from "@/features/admin/users/hooks/use-admin-users";
import { useSuspendUser } from "@/features/admin/users/hooks/use-suspend-user";
import {
  canManageUserStatus,
  formatDateTime,
  getUserRoleLabel,
  getUserStatusBadgeClass,
  getUserStatusLabel,
} from "@/features/admin/users/lib/user-formatters";
import { isApiError } from "@/services/api-client";
import type { AdminUserStatusFilter } from "@/types/admin-user";
import { UserStatusValue } from "@/types/auth";

const PAGE_SIZE = 20;
const SEARCH_DEBOUNCE_MS = 400;

export function UsersTable() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState<AdminUserStatusFilter>("all");
  const [emailInput, setEmailInput] = useState("");
  const [nameInput, setNameInput] = useState("");
  const [emailSearch, setEmailSearch] = useState("");
  const [nameSearch, setNameSearch] = useState("");
  const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
  const [isDetailOpen, setIsDetailOpen] = useState(false);
  const [actionError, setActionError] = useState<string | null>(null);
  const [actingUserId, setActingUserId] = useState<string | null>(null);

  useEffect(() => {
    const timer = window.setTimeout(() => {
      setEmailSearch(emailInput.trim());
      setNameSearch(nameInput.trim());
      setPage(1);
    }, SEARCH_DEBOUNCE_MS);

    return () => window.clearTimeout(timer);
  }, [emailInput, nameInput]);

  const status =
    statusFilter === "all" ? undefined : statusFilter;

  const { data, isLoading, isError, error, isFetching } = useAdminUsers({
    page,
    pageSize: PAGE_SIZE,
    email: emailSearch || undefined,
    name: nameSearch || undefined,
    status,
  });

  const suspendUser = useSuspendUser();
  const activateUser = useActivateUser();

  const handleStatusFilterChange = (value: AdminUserStatusFilter) => {
    setStatusFilter(value);
    setPage(1);
  };

  const handleViewDetail = (userId: string) => {
    setSelectedUserId(userId);
    setIsDetailOpen(true);
  };

  const handleDetailOpenChange = (open: boolean) => {
    setIsDetailOpen(open);

    if (!open) {
      setSelectedUserId(null);
    }
  };

  const handleSuspend = async (userId: string) => {
    setActionError(null);
    setActingUserId(userId);

    try {
      await suspendUser.mutateAsync(userId);
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    } finally {
      setActingUserId(null);
    }
  };

  const handleActivate = async (userId: string) => {
    setActionError(null);
    setActingUserId(userId);

    try {
      await activateUser.mutateAsync(userId);
    } catch (actionErr) {
      setActionError(getErrorMessage(actionErr));
    } finally {
      setActingUserId(null);
    }
  };

  const isActionPending =
    suspendUser.isPending || activateUser.isPending;

  return (
    <>
      <Card>
        <CardHeader className="gap-4">
          <div>
            <CardTitle>Users</CardTitle>
            <CardDescription>
              Manage platform users, review account status, and take moderation actions.
            </CardDescription>
          </div>

          <UserSearchFilters
            email={emailInput}
            name={nameInput}
            onEmailChange={setEmailInput}
            onNameChange={setNameInput}
            disabled={isLoading || isFetching}
          />

          <UserStatusFilter
            value={statusFilter}
            onChange={handleStatusFilterChange}
            disabled={isLoading || isFetching}
          />
        </CardHeader>

        <CardContent className="space-y-4">
          {actionError ? (
            <p className="text-sm text-destructive" role="alert">
              {actionError}
            </p>
          ) : null}

          {isLoading ? (
            <div className="flex items-center justify-center py-12 text-sm text-muted-foreground">
              <Loader2 className="mr-2 size-4 animate-spin" />
              Loading users...
            </div>
          ) : null}

          {isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(error)}
            </p>
          ) : null}

          {!isLoading && !isError && data?.items.length === 0 ? (
            <p className="py-12 text-center text-sm text-muted-foreground">
              No users found matching your filters.
            </p>
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <div className="overflow-x-auto rounded-lg border">
              <table className="w-full min-w-[880px] text-left text-sm">
                <thead className="border-b bg-muted/40">
                  <tr>
                    <th className="px-4 py-3 font-medium">Email</th>
                    <th className="px-4 py-3 font-medium">Name</th>
                    <th className="px-4 py-3 font-medium">Status</th>
                    <th className="px-4 py-3 font-medium">Created date</th>
                    <th className="px-4 py-3 font-medium text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {data.items.map((user) => {
                    const showStatusActions = canManageUserStatus(
                      user.role,
                      user.status,
                    );
                    const isRowActing = actingUserId === user.id && isActionPending;

                    return (
                      <tr key={user.id} className="border-b last:border-b-0">
                        <td className="px-4 py-3">{user.email}</td>
                        <td className="px-4 py-3 font-medium">{user.fullName}</td>
                        <td className="px-4 py-3">
                          <span
                            className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getUserStatusBadgeClass(user.status)}`}
                          >
                            {getUserStatusLabel(user.status)}
                          </span>
                          <span className="mt-1 block text-xs text-muted-foreground">
                            {getUserRoleLabel(user.role)}
                          </span>
                        </td>
                        <td className="whitespace-nowrap px-4 py-3 text-muted-foreground">
                          {formatDateTime(user.createdAt)}
                        </td>
                        <td className="px-4 py-3">
                          <div className="flex flex-wrap justify-end gap-2">
                            <Button
                              type="button"
                              variant="outline"
                              size="sm"
                              onClick={() => handleViewDetail(user.id)}
                            >
                              <Eye />
                              View
                            </Button>

                            {showStatusActions &&
                            user.status === UserStatusValue.Active ? (
                              <Button
                                type="button"
                                variant="outline"
                                size="sm"
                                disabled={isRowActing}
                                onClick={() => void handleSuspend(user.id)}
                              >
                                <UserX />
                                Suspend
                              </Button>
                            ) : null}

                            {showStatusActions &&
                            user.status === UserStatusValue.Suspended ? (
                              <Button
                                type="button"
                                variant="outline"
                                size="sm"
                                disabled={isRowActing}
                                onClick={() => void handleActivate(user.id)}
                              >
                                <UserCheck />
                                Activate
                              </Button>
                            ) : null}
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : null}

          {!isLoading && !isError && data && data.totalCount > 0 ? (
            <UsersPagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
              disabled={isFetching}
            />
          ) : null}
        </CardContent>
      </Card>

      <UserDetailModal
        userId={selectedUserId}
        open={isDetailOpen}
        onOpenChange={handleDetailOpenChange}
      />
    </>
  );
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    return error.response?.data?.message ?? "Unable to complete the request.";
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to complete the request.";
}
