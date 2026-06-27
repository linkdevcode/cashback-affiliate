"use client";

import { UserCheck, Users, UserX } from "lucide-react";
import { useEffect, useState } from "react";

import { ConfirmDialog } from "@/components/confirm-dialog";
import { EmptyState } from "@/components/empty-state";
import { RowActionsMenu } from "@/components/row-actions-menu";
import { UserStatusBadge } from "@/components/status-badge";
import { TableFetchOverlay } from "@/components/table-fetch-overlay";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { AdminUserFilters } from "@/features/admin/users/components/admin-user-filters";
import { UserDetailModal } from "@/features/admin/users/components/user-detail-modal";
import { useActivateUser } from "@/features/admin/users/hooks/use-activate-user";
import { useAdminUsers } from "@/features/admin/users/hooks/use-admin-users";
import { useSuspendUser } from "@/features/admin/users/hooks/use-suspend-user";
import {
  canManageUserStatus,
  formatDateTime,
  getUserRoleLabel,
  getUserStatusLabel,
} from "@/features/admin/users/lib/user-formatters";
import { OrdersPagination } from "@/features/orders/components/orders-pagination";
import { isApiError } from "@/services/api-client";
import type { AdminUser, AdminUserStatusFilter } from "@/types/admin-user";
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
  const [confirmSuspendUserId, setConfirmSuspendUserId] = useState<string | null>(null);

  useEffect(() => {
    const timer = window.setTimeout(() => {
      setEmailSearch(emailInput.trim());
      setNameSearch(nameInput.trim());
      setPage(1);
    }, SEARCH_DEBOUNCE_MS);

    return () => window.clearTimeout(timer);
  }, [emailInput, nameInput]);

  const status = statusFilter === "all" ? undefined : statusFilter;

  const { data, isLoading, isError, error, isFetching } = useAdminUsers({
    page,
    pageSize: PAGE_SIZE,
    email: emailSearch || undefined,
    name: nameSearch || undefined,
    status,
  });

  const suspendUser = useSuspendUser();
  const activateUser = useActivateUser();

  const hasActiveFilters =
    Boolean(emailSearch || nameSearch) || statusFilter !== "all";
  const isEmpty = !isLoading && !isError && data?.items.length === 0;
  const isActionPending = suspendUser.isPending || activateUser.isPending;

  const handleClearFilters = () => {
    setStatusFilter("all");
    setEmailInput("");
    setNameInput("");
    setEmailSearch("");
    setNameSearch("");
    setPage(1);
  };

  const handleSuspend = async (userId: string) => {
    setActionError(null);
    setActingUserId(userId);

    try {
      await suspendUser.mutateAsync(userId);
      setConfirmSuspendUserId(null);
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

          <AdminUserFilters
            email={emailInput}
            name={nameInput}
            statusFilter={statusFilter}
            onEmailChange={setEmailInput}
            onNameChange={setNameInput}
            onStatusFilterChange={(value) => {
              setStatusFilter(value);
              setPage(1);
            }}
            onClearFilters={handleClearFilters}
            disabled={isLoading}
          />
        </CardHeader>

        <CardContent className="space-y-4">
          {actionError ? (
            <p className="text-sm text-destructive" role="alert">
              {actionError}
            </p>
          ) : null}

          {isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(error)}
            </p>
          ) : null}

          {isLoading ? <UsersTableSkeleton /> : null}

          {isEmpty && !hasActiveFilters ? (
            <EmptyState
              icon={Users}
              title="No users yet"
              description="Registered user accounts will appear here."
            />
          ) : null}

          {isEmpty && hasActiveFilters ? (
            <EmptyState
              icon={Users}
              title="No matching users"
              description="No users found matching your filters."
              action={
                <Button type="button" variant="ghost" size="sm" onClick={handleClearFilters}>
                  Clear filters
                </Button>
              }
            />
          ) : null}

          {!isLoading && !isError && data && data.items.length > 0 ? (
            <TableFetchOverlay isFetching={isFetching}>
              <UsersDesktopTable
                users={data.items}
                actingUserId={actingUserId}
                isActionPending={isActionPending}
                onViewDetail={(id) => {
                  setSelectedUserId(id);
                  setIsDetailOpen(true);
                }}
                onRequestSuspend={setConfirmSuspendUserId}
                onActivate={handleActivate}
              />
              <UsersMobileList
                users={data.items}
                actingUserId={actingUserId}
                isActionPending={isActionPending}
                onViewDetail={(id) => {
                  setSelectedUserId(id);
                  setIsDetailOpen(true);
                }}
                onRequestSuspend={setConfirmSuspendUserId}
                onActivate={handleActivate}
              />
            </TableFetchOverlay>
          ) : null}
        </CardContent>

        {!isLoading && !isError && data && data.totalCount > 0 ? (
          <CardFooter className="border-t">
            <OrdersPagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              onPageChange={setPage}
              disabled={isFetching}
            />
          </CardFooter>
        ) : null}
      </Card>

      <UserDetailModal
        userId={selectedUserId}
        open={isDetailOpen}
        onOpenChange={(open) => {
          setIsDetailOpen(open);
          if (!open) setSelectedUserId(null);
        }}
      />

      <ConfirmDialog
        open={Boolean(confirmSuspendUserId)}
        onOpenChange={(open) => !open && setConfirmSuspendUserId(null)}
        title="Suspend user?"
        description="The user will not be able to log in, generate links, or request withdrawals."
        confirmLabel="Suspend user"
        destructive
        loading={isActionPending}
        onConfirm={() => {
          if (confirmSuspendUserId) {
            void handleSuspend(confirmSuspendUserId);
          }
        }}
      />
    </>
  );
}

interface UsersListProps {
  users: AdminUser[];
  actingUserId: string | null;
  isActionPending: boolean;
  onViewDetail: (id: string) => void;
  onRequestSuspend: (id: string) => void;
  onActivate: (id: string) => void;
}

function UsersDesktopTable({
  users,
  actingUserId,
  isActionPending,
  onViewDetail,
  onRequestSuspend,
  onActivate,
}: UsersListProps) {
  return (
    <div className="hidden overflow-hidden rounded-xl border border-border md:block">
      <table className="w-full text-left text-sm">
        <thead className="sticky top-0 z-10 border-b border-border bg-muted/40">
          <tr>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Email</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Name</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Status</th>
            <th scope="col" className="px-4 py-3 text-xs font-medium text-muted-foreground">Created</th>
            <th scope="col" className="px-4 py-3 text-right text-xs font-medium text-muted-foreground">Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id} className="border-b border-border last:border-b-0 hover:bg-muted/30">
              <td className="px-4 py-3 text-sm">{user.email}</td>
              <td className="px-4 py-3 font-medium">{user.fullName}</td>
              <td className="px-4 py-3">
                <UserStatusBadge status={user.status} label={getUserStatusLabel(user.status)} />
                <span className="mt-1 block text-xs text-muted-foreground">
                  {getUserRoleLabel(user.role)}
                </span>
              </td>
              <td className="px-4 py-3 text-xs text-muted-foreground">{formatDateTime(user.createdAt)}</td>
              <td className="px-4 py-3">
                <UserRowActions
                  user={user}
                  actingUserId={actingUserId}
                  isActionPending={isActionPending}
                  onViewDetail={onViewDetail}
                  onRequestSuspend={onRequestSuspend}
                  onActivate={onActivate}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function UsersMobileList(props: UsersListProps) {
  return (
    <div className="space-y-3 md:hidden">
      {props.users.map((user) => (
        <article key={user.id} className="space-y-3 rounded-xl border border-border p-4">
          <div className="flex items-start justify-between gap-3">
            <div className="min-w-0">
              <p className="truncate font-medium">{user.fullName}</p>
              <p className="truncate text-xs text-muted-foreground">{user.email}</p>
            </div>
            <UserStatusBadge
              status={user.status}
              label={getUserStatusLabel(user.status)}
              className="shrink-0"
            />
          </div>
          <p className="text-xs text-muted-foreground">
            {getUserRoleLabel(user.role)} · {formatDateTime(user.createdAt)}
          </p>
          <UserRowActions {...props} user={user} align="start" />
        </article>
      ))}
    </div>
  );
}

function UserRowActions({
  user,
  actingUserId,
  isActionPending,
  onViewDetail,
  onRequestSuspend,
  onActivate,
  align = "end",
}: {
  user: AdminUser;
  actingUserId: string | null;
  isActionPending: boolean;
  onViewDetail: (id: string) => void;
  onRequestSuspend: (id: string) => void;
  onActivate: (id: string) => void;
  align?: "end" | "start";
}) {
  const showStatusActions = canManageUserStatus(user.role, user.status);
  const isRowActing = actingUserId === user.id && isActionPending;

  const menuActions = [
    { label: "View details", onClick: () => onViewDetail(user.id) },
  ];

  return (
    <div className={`flex flex-wrap gap-2 ${align === "end" ? "justify-end" : "justify-start"}`}>
      {showStatusActions && user.status === UserStatusValue.Suspended ? (
        <Button
          type="button"
          variant="default"
          size="sm"
          disabled={isRowActing}
          onClick={() => void onActivate(user.id)}
        >
          <UserCheck className="size-4" />
          Activate
        </Button>
      ) : null}

      {showStatusActions && user.status === UserStatusValue.Active ? (
        <Button
          type="button"
          variant="destructive"
          size="sm"
          disabled={isRowActing}
          onClick={() => onRequestSuspend(user.id)}
        >
          <UserX className="size-4" />
          Suspend
        </Button>
      ) : null}

      <RowActionsMenu actions={menuActions} disabled={isRowActing} />
    </div>
  );
}

function UsersTableSkeleton() {
  return (
    <>
      <div className="hidden overflow-hidden rounded-xl border border-border md:block">
        <div className="border-b border-border bg-muted/40 px-4 py-3">
          <Skeleton className="h-3 w-full max-w-lg" />
        </div>
        {Array.from({ length: 5 }).map((_, index) => (
          <div key={index} className="flex gap-4 border-b border-border px-4 py-3 last:border-b-0">
            <Skeleton className="h-4 w-40" />
            <Skeleton className="h-4 w-28" />
            <Skeleton className="h-4 w-16" />
            <Skeleton className="h-4 w-28" />
            <Skeleton className="ml-auto h-8 w-20" />
          </div>
        ))}
      </div>
      <div className="space-y-3 md:hidden">
        {Array.from({ length: 3 }).map((_, index) => (
          <Skeleton key={index} className="h-32 w-full rounded-xl" />
        ))}
      </div>
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
