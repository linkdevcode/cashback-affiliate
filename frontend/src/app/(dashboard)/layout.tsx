import type { ReactNode } from "react";

import { DashboardLayout } from "@/components/layouts/dashboard-layout";

export default function DashboardRouteLayout({
  children,
}: {
  children: ReactNode;
}) {
  return <DashboardLayout>{children}</DashboardLayout>;
}
