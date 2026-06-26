import { EarningsSummaryCards } from "@/features/orders/components/earnings-summary-cards";
import { OrdersTable } from "@/features/orders/components/orders-table";

export default function OrdersPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Orders</h1>
        <p className="mt-2 text-sm text-muted-foreground">
          View your order history, track cashback earnings, and monitor conversion status.
        </p>
      </div>

      <EarningsSummaryCards />
      <OrdersTable />
    </div>
  );
}
