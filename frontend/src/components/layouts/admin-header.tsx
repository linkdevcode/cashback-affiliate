"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useRouter } from "next/navigation";

import { AdminMobileNav } from "@/components/layouts/admin-mobile-nav";
import { Button } from "@/components/ui/button";
import { getAdminNavItem } from "@/config/admin-navigation";
import { useAuth } from "@/providers/auth-provider";

export function AdminHeader() {
  const pathname = usePathname();
  const router = useRouter();
  const { user, logout } = useAuth();

  const activeNavItem = getAdminNavItem(pathname);
  const pageTitle = activeNavItem?.title ?? "Quản trị hệ thống";

  const handleLogout = () => {
    logout();
    router.replace("/login");
  };

  return (
    <header className="sticky top-0 z-30">
      <div className="flex h-16 items-center justify-between border-b border-border bg-background/60 backdrop-blur-sm px-4 sm:px-6">
        <div className="max-w-7xl mx-auto w-full px-2 sm:px-0 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <AdminMobileNav />
            <div className="min-w-0">
              <h1 className="truncate text-lg font-semibold tracking-tight">{pageTitle}</h1>
            </div>
            <Link
              href="/dashboard"
              className="hidden text-sm text-muted-foreground transition-colors hover:text-foreground sm:inline"
            >
              Quay lại ứng dụng
            </Link>
          </div>

          <div className="flex items-center gap-3">
            {user ? (
              <span className="hidden max-w-[8rem] truncate text-sm text-muted-foreground sm:inline sm:max-w-none">
                {user.fullName}
              </span>
            ) : null}
            <Button variant="outline" size="sm" onClick={handleLogout}>
              Đăng xuất
            </Button>
          </div>
        </div>
      </div>
    </header>
  );
}
