import type { ReactNode } from "react";

import { AdminHeader } from "@/components/layouts/admin-header";
import { AdminSidebar } from "@/components/layouts/admin-sidebar";

interface AdminLayoutProps {
  children: ReactNode;
}

export function AdminLayout({ children }: AdminLayoutProps) {
  return (
    <div className="flex min-h-screen bg-background">
      <AdminSidebar />
      <div className="flex min-h-screen flex-1 flex-col">
        <AdminHeader />
        <main className="flex-1 p-4 sm:p-6">{children}</main>
      </div>
    </div>
  );
}
