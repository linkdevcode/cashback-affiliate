import { EarningsSummaryCards } from "@/features/orders/components/earnings-summary-cards";
import { OrdersTable } from "@/features/orders/components/orders-table";

export default function OrdersPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        View your order history, track cashback earnings, and monitor conversion status.
      </p>

      <EarningsSummaryCards />
      <OrdersTable />
    </div>
  );
}
