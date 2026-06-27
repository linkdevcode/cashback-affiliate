import type { ReactNode } from "react";

import { DashboardHeader } from "@/components/layouts/dashboard-header";
import { DashboardMobileNav } from "@/components/layouts/dashboard-mobile-nav";
import { DashboardSidebar } from "@/components/layouts/dashboard-sidebar";

interface DashboardLayoutProps {
  children: ReactNode;
}

export function DashboardLayout({ children }: DashboardLayoutProps) {
  return (
    <div className="flex min-h-screen bg-background">
      <DashboardSidebar />
      <div className="flex min-h-screen flex-1 flex-col">
        <DashboardHeader />
        <main className="flex-1 p-4 pb-20 sm:p-6 md:pb-6">{children}</main>
        <DashboardMobileNav />
      </div>
    </div>
  );
}
