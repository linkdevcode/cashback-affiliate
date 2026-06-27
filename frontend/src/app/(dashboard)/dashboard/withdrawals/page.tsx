import { WithdrawalForm } from "@/features/withdrawals/components/withdrawal-form";
import { WithdrawalsTable } from "@/features/withdrawals/components/withdrawals-table";

export default function WithdrawalsPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Request a withdrawal from your available balance and track processing status.
      </p>

      <WithdrawalForm />
      <WithdrawalsTable />
    </div>
  );
}
