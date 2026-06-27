import { AdminDashboardOverview } from "@/features/admin/dashboard/components/admin-dashboard-overview";

export default function AdminDashboardPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Platform overview with user, order, withdrawal, and revenue statistics.
      </p>

      <AdminDashboardOverview />
    </div>
  );
}
