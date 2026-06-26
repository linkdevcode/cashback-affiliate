"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

const navigationItems = [
  { label: "Dashboard", href: "/dashboard" },
  { label: "Affiliate Links", href: "/dashboard/affiliate" },
  { label: "Orders", href: "/dashboard/orders" },
  { label: "Withdrawals", href: "#" },
] as const;

export function DashboardSidebar() {
  const pathname = usePathname();

  return (
    <aside className="hidden w-64 shrink-0 border-r bg-sidebar md:block">
      <div className="flex h-16 items-center border-b px-6">
        <span className="text-base font-semibold">Cashback</span>
      </div>

      <nav className="space-y-1 p-4">
        {navigationItems.map((item) => {
          const isActive =
            item.href !== "#" &&
            (pathname === item.href || pathname.startsWith(`${item.href}/`));

          return (
            <Link
              key={item.label}
              href={item.href}
              className={`block rounded-md px-3 py-2 text-sm transition-colors ${
                isActive
                  ? "bg-sidebar-accent text-sidebar-accent-foreground"
                  : "text-muted-foreground hover:bg-muted hover:text-foreground"
              }`}
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
