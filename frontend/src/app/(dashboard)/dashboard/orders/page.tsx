import { EarningsSummaryCards } from "@/features/orders/components/earnings-summary-cards";
import { OrdersTable } from "@/features/orders/components/orders-table";

export default function OrdersPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Xem lịch sử đơn hàng, theo dõi khoản hoàn tiền và giám sát trạng thái chuyển đổi.
      </p>

      <EarningsSummaryCards />
      <OrdersTable />
    </div>
  );
}
