import { AdminOrdersTable } from "@/features/admin/orders/components/admin-orders-table";

export default function AdminOrdersPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Order management</h1>
        <p className="mt-2 text-sm text-muted-foreground">
          Search and filter platform orders, then review conversion details for each user.
        </p>
      </div>

      <AdminOrdersTable />
    </div>
  );
}
