import { WithdrawalForm } from "@/features/withdrawals/components/withdrawal-form";
import { WithdrawalsTable } from "@/features/withdrawals/components/withdrawals-table";

export default function WithdrawalsPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Yêu cầu rút tiền từ số dư khả dụng và theo dõi trạng thái xử lý.
      </p>

      <WithdrawalForm />
      <WithdrawalsTable />
    </div>
  );
}
