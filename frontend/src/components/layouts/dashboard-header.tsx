"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useRouter } from "next/navigation";

import { Button } from "@/components/ui/button";
import {
  dashboardNavigationItems,
  getDashboardNavItem,
} from "@/config/dashboard-navigation";
import { useAuth } from "@/providers/auth-provider";
import { cn } from "@/lib/utils";

export function DashboardHeader() {
  const pathname = usePathname();
  const router = useRouter();
  const { user, logout } = useAuth();

  const activeNavItem = getDashboardNavItem(pathname);
  const pageTitle = activeNavItem?.title ?? "Dashboard";
  const pageDescription = activeNavItem?.description;
  const effectivePath = pathname === "/dashboard" ? "/dashboard/workspace" : pathname;

  const handleLogout = () => {
    logout();
    router.replace("/login");
  };

  return (
    <header className="border-b border-border bg-background/95 backdrop-blur-xl">
      <div className="mx-auto flex max-w-7xl flex-col gap-4 px-4 py-4 sm:px-6">
        <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          <div className="space-y-1">
            <h1 className="text-lg font-semibold tracking-tight text-foreground">
              {pageTitle}
            </h1>
            {pageDescription ? (
              <p className="text-sm text-muted-foreground">{pageDescription}</p>
            ) : null}
          </div>

          <div className="flex items-center gap-3">
            {user ? (
              <span className="max-w-[8rem] truncate text-sm text-muted-foreground sm:max-w-none">
                {user.fullName}
              </span>
            ) : null}
            <Button variant="outline" size="sm" onClick={handleLogout}>
              Logout
            </Button>
          </div>
        </div>

        <nav className="hidden items-center gap-2 overflow-x-auto rounded-full border border-border bg-card p-1 shadow-sm md:flex">
          {dashboardNavigationItems.map((item) => {
            const isActive =
              effectivePath === item.href ||
              effectivePath.startsWith(`${item.href}/`);

            return (
              <Link
                key={item.href}
                href={item.href}
                className={cn(
                  "rounded-full px-4 py-2 text-sm font-medium transition-colors",
                  isActive
                    ? "bg-brand text-white shadow"
                    : "text-muted-foreground hover:text-foreground",
                )}
                aria-current={isActive ? "page" : undefined}
              >
                {item.label}
              </Link>
            );
          })}
        </nav>
      </div>
    </header>
  );
}
