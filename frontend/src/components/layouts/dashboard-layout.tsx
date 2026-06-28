import type { ReactNode } from "react";

import { DashboardHeader } from "@/components/layouts/dashboard-header";
import { DashboardMobileNav } from "@/components/layouts/dashboard-mobile-nav";

interface DashboardLayoutProps {
  children: ReactNode;
}

export function DashboardLayout({ children }: DashboardLayoutProps) {
  return (
    <div className="min-h-screen bg-background">
      <DashboardHeader />
      <main className="min-h-screen p-4 pb-20 sm:p-6 md:pb-6">{children}</main>
      <DashboardMobileNav />
    </div>
  );
}
