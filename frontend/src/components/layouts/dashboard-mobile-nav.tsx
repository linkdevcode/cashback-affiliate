"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { LayoutDashboard, Link2, Package, Wallet, Tag } from "lucide-react";

import { dashboardNavigationItems } from "@/config/dashboard-navigation";
import { cn } from "@/lib/utils";

const mobileNavIcons = {
  "/dashboard/workspace": LayoutDashboard,
  "/dashboard/affiliate": Link2,
  "/dashboard/campaigns": Tag,
  "/dashboard/orders": Package,
  "/dashboard/withdrawals": Wallet,
} as const;

export function DashboardMobileNav() {
  const pathname = usePathname();

  return (
    <nav
      aria-label="Mobile navigation"
      className="fixed inset-x-0 bottom-0 z-50 border-t border-border bg-background md:hidden"
    >
      <ul className="grid grid-cols-4">
        {dashboardNavigationItems.map((item) => {
          const isActive =
            item.href === "/dashboard"
              ? pathname === "/dashboard"
              : pathname === item.href || pathname.startsWith(`${item.href}/`);

          const Icon = mobileNavIcons[item.href as keyof typeof mobileNavIcons];

          return (
            <li key={item.href}>
              <Link
                href={item.href}
                className={cn(
                  "flex flex-col items-center gap-1 px-2 py-3 text-xs font-medium transition-colors",
                  isActive
                    ? "text-brand"
                    : "text-muted-foreground hover:text-foreground",
                )}
                aria-current={isActive ? "page" : undefined}
              >
                <Icon className="size-5" aria-hidden="true" />
                <span className="truncate">{item.label.split(" ")[0]}</span>
              </Link>
            </li>
          );
        })}
      </ul>
    </nav>
  );
}
