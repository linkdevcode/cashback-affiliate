import { AdminWithdrawalsTable } from "@/features/admin/withdrawals/components/admin-withdrawals-table";

export default function AdminWithdrawalsPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Review withdrawal requests, filter by status, and approve or reject payouts.
      </p>

      <AdminWithdrawalsTable />
    </div>
  );
}
