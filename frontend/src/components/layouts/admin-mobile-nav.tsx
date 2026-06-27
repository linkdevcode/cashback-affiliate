"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { LayoutDashboard, Menu, Package, Users, Wallet, X } from "lucide-react";
import { useState } from "react";

import { adminNavigationItems } from "@/config/admin-navigation";
import { cn } from "@/lib/utils";

const adminNavIcons = {
  "/admin": LayoutDashboard,
  "/admin/users": Users,
  "/admin/orders": Package,
  "/admin/withdrawals": Wallet,
} as const;

export function AdminMobileNav() {
  const pathname = usePathname();
  const [isOpen, setIsOpen] = useState(false);

  return (
    <>
      <button
        type="button"
        className="rounded-lg p-2 text-muted-foreground hover:bg-muted hover:text-foreground md:hidden"
        aria-label="Open admin navigation"
        aria-expanded={isOpen}
        onClick={() => setIsOpen(true)}
      >
        <Menu className="size-5" />
      </button>

      {isOpen ? (
        <div className="fixed inset-0 z-50 md:hidden">
          <button
            type="button"
            className="absolute inset-0 bg-black/50"
            aria-label="Close navigation"
            onClick={() => setIsOpen(false)}
          />
          <aside className="absolute inset-y-0 left-0 flex w-72 flex-col border-r border-border bg-sidebar shadow-lg">
            <div className="flex h-16 items-center justify-between border-b border-border px-4">
              <span className="text-base font-semibold">Admin</span>
              <button
                type="button"
                className="rounded-lg p-2 text-muted-foreground hover:bg-muted"
                aria-label="Close navigation"
                onClick={() => setIsOpen(false)}
              >
                <X className="size-5" />
              </button>
            </div>
            <nav className="space-y-1 p-4">
              {adminNavigationItems.map((item) => {
                const isActive =
                  item.href === "/admin"
                    ? pathname === "/admin"
                    : pathname === item.href || pathname.startsWith(`${item.href}/`);

                const Icon = adminNavIcons[item.href as keyof typeof adminNavIcons];

                return (
                  <Link
                    key={item.href}
                    href={item.href}
                    className={cn(
                      "flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors",
                      isActive
                        ? "bg-brand-muted text-brand"
                        : "text-muted-foreground hover:bg-muted hover:text-foreground",
                    )}
                    aria-current={isActive ? "page" : undefined}
                    onClick={() => setIsOpen(false)}
                  >
                    <Icon className="size-4 shrink-0" aria-hidden="true" />
                    {item.label}
                  </Link>
                );
              })}
            </nav>
          </aside>
        </div>
      ) : null}
    </>
  );
}
