"use client";

import type { ReactNode } from "react";

import { ProtectedRoute } from "@/components/routes/protected-route";
import { DashboardLayout } from "@/components/layouts/dashboard-layout";

export default function DashboardRouteLayout({
  children,
}: {
  children: ReactNode;
}) {
  return (
    <ProtectedRoute>
      <DashboardLayout>{children}</DashboardLayout>
    </ProtectedRoute>
  );
}
