import type { AdminUserStatusFilter } from "@/types/admin-user";
import { UserRoleValue, UserStatusValue, type UserRole, type UserStatus } from "@/types/auth";

export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
    maximumFractionDigits: 0,
  }).format(amount);
}

export function formatDateTime(value: string): string {
  return new Intl.DateTimeFormat("vi-VN", {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(value));
}

export function getUserStatusLabel(status: UserStatus): string {
  switch (status) {
    case UserStatusValue.Active:
      return "Active";
    case UserStatusValue.Suspended:
      return "Suspended";
    case UserStatusValue.Deleted:
      return "Deleted";
    default:
      return "Unknown";
  }
}

export function getUserStatusFilterLabel(filter: AdminUserStatusFilter): string {
  switch (filter) {
    case "all":
      return "All";
    case UserStatusValue.Active:
      return "Active";
    case UserStatusValue.Suspended:
      return "Suspended";
    default:
      return "All";
  }
}

export function getUserStatusBadgeClass(status: UserStatus): string {
  switch (status) {
    case UserStatusValue.Active:
      return "bg-success-muted text-success";
    case UserStatusValue.Suspended:
      return "bg-destructive-muted text-destructive";
    case UserStatusValue.Deleted:
      return "bg-muted text-muted-foreground";
    default:
      return "bg-muted text-muted-foreground";
  }
}

export function getUserRoleLabel(role: UserRole): string {
  switch (role) {
    case UserRoleValue.Admin:
      return "Admin";
    case UserRoleValue.User:
      return "User";
    default:
      return "Unknown";
  }
}

export function canManageUserStatus(role: UserRole, status: UserStatus): boolean {
  return role !== UserRoleValue.Admin && status !== UserStatusValue.Deleted;
}
