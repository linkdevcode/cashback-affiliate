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

export function getWithdrawalStatusDescription(status: WithdrawalStatus): string {
  switch (status) {
    case WithdrawalStatus.Pending:
      return "Awaiting admin review";
    case WithdrawalStatus.Approved:
      return "Approved — transfer pending";
    case WithdrawalStatus.Completed:
      return "Paid to your bank account";
    case WithdrawalStatus.Rejected:
      return "Balance restored";
    default:
      return "";
  }
}

export function getWithdrawalStatusBadgeClass(status: WithdrawalStatus): string {
  switch (status) {
    case WithdrawalStatus.Pending:
      return "bg-warning-muted text-warning";
    case WithdrawalStatus.Approved:
      return "bg-info-muted text-info";
    case WithdrawalStatus.Rejected:
      return "bg-destructive-muted text-destructive";
    case WithdrawalStatus.Completed:
      return "bg-success-muted text-success";
    default:
      return "bg-muted text-muted-foreground";
  }
}

export function formatWithdrawalReference(id: string): string {
  return id.slice(0, 8).toUpperCase();
}
