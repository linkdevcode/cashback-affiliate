"use client";

import type { ReactNode } from "react";

import { AdminLayout } from "@/components/layouts/admin-layout";
import { AdminRoute } from "@/components/routes/admin-route";

export default function AdminRouteLayout({
  children,
}: {
  children: ReactNode;
}) {
  return (
    <AdminRoute>
      <AdminLayout>{children}</AdminLayout>
    </AdminRoute>
  );
}
