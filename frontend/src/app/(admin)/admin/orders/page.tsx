import { AdminOrdersTable } from "@/features/admin/orders/components/admin-orders-table";

export default function AdminOrdersPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Search and filter platform orders, then review conversion details for each user.
      </p>

      <AdminOrdersTable />
    </div>
  );
}
