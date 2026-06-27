import { DashboardOverview } from "@/features/dashboard/components/dashboard-overview";

export default function DashboardPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Track your available balance, pending cashback, and withdrawal history.
      </p>

      <DashboardOverview />
    </div>
  );
}
