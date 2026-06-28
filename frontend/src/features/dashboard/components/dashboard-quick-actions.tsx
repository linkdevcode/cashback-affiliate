import Link from "next/link";
import { Link2, Wallet } from "lucide-react";

import { buttonVariants } from "@/components/ui/button";
import { cn } from "@/lib/utils";

export function DashboardQuickActions() {
  return (
    <div className="flex flex-col gap-3 sm:flex-row">
      <Link
        href="/dashboard/affiliate"
        className={cn(buttonVariants(), "sm:flex-1")}
      >
        <Link2 />
        Generate link
      </Link>
      <Link
        href="/dashboard/withdrawals"
        className={cn(buttonVariants({ variant: "outline" }), "sm:flex-1")}
      >
        <Wallet />
        Request withdrawal
      </Link>
    </div>
  );
}
