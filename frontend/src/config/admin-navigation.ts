export interface AdminNavItem {
  label: string;
  href: string;
  title: string;
  description: string;
}

export const adminNavigationItems: AdminNavItem[] = [
  {
    label: "Dashboard",
    href: "/admin",
    title: "Admin dashboard",
    description: "Platform overview with user, order, withdrawal, and revenue statistics.",
  },
  {
    label: "Users",
    href: "/admin/users",
    title: "User management",
    description: "Search users, review account details, and manage active or suspended status.",
  },
  {
    label: "Orders",
    href: "/admin/orders",
    title: "Order management",
    description: "Search and filter platform orders, then review conversion details for each user.",
  },
  {
    label: "Withdrawals",
    href: "/admin/withdrawals",
    title: "Withdrawal management",
    description: "Review withdrawal requests, filter by status, and approve or reject payouts.",
  },
];

export function getAdminNavItem(pathname: string): AdminNavItem | undefined {
  return adminNavigationItems.find((item) => {
    if (item.href === "/admin") {
      return pathname === "/admin";
    }

    return pathname === item.href || pathname.startsWith(`${item.href}/`);
  });
}
