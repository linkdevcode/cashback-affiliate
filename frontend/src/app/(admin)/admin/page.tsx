import { AdminDashboardOverview } from "@/features/admin/dashboard/components/admin-dashboard-overview";

export default function AdminDashboardPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Admin dashboard</h1>
        <p className="mt-2 text-sm text-muted-foreground">
          Platform overview with user, order, withdrawal, and revenue statistics.
        </p>
      </div>

      <AdminDashboardOverview />
    </div>
  );
}
