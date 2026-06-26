"use client";

import { useRouter } from "next/navigation";

import { Button } from "@/components/ui/button";
import { useAuth } from "@/providers/auth-provider";

export function DashboardHeader() {
  const router = useRouter();
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
    router.replace("/login");
  };

  return (
    <header className="flex h-16 items-center justify-between border-b bg-background px-4 sm:px-6">
      <h1 className="text-lg font-semibold">Dashboard</h1>

      <div className="flex items-center gap-3">
        {user ? (
          <span className="hidden text-sm text-muted-foreground sm:inline">
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
