"use client";

import { usePathname } from "next/navigation";
import { useRouter } from "next/navigation";

import { Button } from "@/components/ui/button";
import { getDashboardNavItem } from "@/config/dashboard-navigation";
import { useAuth } from "@/providers/auth-provider";

export function DashboardHeader() {
  const pathname = usePathname();
  const router = useRouter();
  const { user, logout } = useAuth();

  const activeNavItem = getDashboardNavItem(pathname);
  const pageTitle = activeNavItem?.title ?? "Dashboard";

  const handleLogout = () => {
    logout();
    router.replace("/login");
  };

  return (
    <header className="flex h-16 items-center justify-between border-b border-border bg-background px-4 sm:px-6">
      <h1 className="text-lg font-semibold tracking-tight">{pageTitle}</h1>

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
    </header>
  );
}
