import { WithdrawalStatus, type WithdrawalStatusFilter } from "@/types/withdrawal";

export function getWithdrawalStatusLabel(status: WithdrawalStatusFilter): string {
  switch (status) {
    case "all":
      return "All";
    case WithdrawalStatus.Pending:
      return "Pending";
    case WithdrawalStatus.Approved:
      return "Approved";
    case WithdrawalStatus.Rejected:
      return "Rejected";
    case WithdrawalStatus.Completed:
      return "Completed";
    default:
      return "All";
  }
}

export function getWithdrawalStatusBadgeClass(status: WithdrawalStatus): string {
  switch (status) {
    case WithdrawalStatus.Pending:
      return "bg-amber-100 text-amber-800 dark:bg-amber-950 dark:text-amber-300";
    case WithdrawalStatus.Approved:
      return "bg-blue-100 text-blue-800 dark:bg-blue-950 dark:text-blue-300";
    case WithdrawalStatus.Rejected:
      return "bg-rose-100 text-rose-800 dark:bg-rose-950 dark:text-rose-300";
    case WithdrawalStatus.Completed:
      return "bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300";
    default:
      return "bg-muted text-muted-foreground";
  }
}
