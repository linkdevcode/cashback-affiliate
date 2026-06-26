import { WithdrawalForm } from "@/features/withdrawals/components/withdrawal-form";
import { WithdrawalsTable } from "@/features/withdrawals/components/withdrawals-table";

export default function WithdrawalsPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Withdrawals</h1>
        <p className="mt-2 text-sm text-muted-foreground">
          Request a withdrawal from your available balance and track processing status.
        </p>
      </div>

      <WithdrawalForm />
      <WithdrawalsTable />
    </div>
  );
}
