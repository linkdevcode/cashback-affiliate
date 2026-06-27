"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

import { dashboardNavigationItems } from "@/config/dashboard-navigation";
import { cn } from "@/lib/utils";

export function DashboardSidebar() {
  const pathname = usePathname();

  return (
    <aside className="hidden w-64 shrink-0 border-r border-border bg-sidebar md:block">
      <div className="flex h-16 items-center border-b border-border px-6">
        <span className="text-base font-semibold tracking-tight text-foreground">
          Cashback
        </span>
      </div>

      <nav className="space-y-1 p-4" aria-label="Main navigation">
        {dashboardNavigationItems.map((item) => {
          const isActive =
            item.href === "/dashboard"
              ? pathname === "/dashboard"
              : pathname === item.href || pathname.startsWith(`${item.href}/`);

          return (
            <Link
              key={item.href}
              href={item.href}
              className={cn(
                "block rounded-lg px-3 py-2 text-sm font-medium transition-colors",
                isActive
                  ? "bg-brand-muted text-brand"
                  : "text-muted-foreground hover:bg-muted hover:text-foreground",
              )}
              aria-current={isActive ? "page" : undefined}
            >
              {item.label}
            </Link>
          );
        })}
      </nav>
    </aside>
  );
}
