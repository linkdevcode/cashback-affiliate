import { AdminWithdrawalsTable } from "@/features/admin/withdrawals/components/admin-withdrawals-table";

export default function AdminWithdrawalsPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Withdrawal management</h1>
        <p className="mt-2 text-sm text-muted-foreground">
          Review withdrawal requests, filter by status, and approve or reject payouts.
        </p>
      </div>

      <AdminWithdrawalsTable />
    </div>
  );
}
