export interface DashboardNavItem {
  label: string;
  href: string;
  title: string;
  description?: string;
}

export const dashboardNavigationItems: DashboardNavItem[] = [
  {
    label: "Dashboard",
    href: "/dashboard",
    title: "Dashboard",
    description: "Track your available balance, pending cashback, and withdrawal history.",
  },
  {
    label: "Affiliate Links",
    href: "/dashboard/affiliate",
    title: "Link Converter",
    description: "Paste a Shopee product URL to create your cashback affiliate link.",
  },
  {
    label: "Orders",
    href: "/dashboard/orders",
    title: "Orders",
    description: "View your order history, track cashback earnings, and monitor conversion status.",
  },
  {
    label: "Withdrawals",
    href: "/dashboard/withdrawals",
    title: "Withdrawals",
    description: "Request a withdrawal from your available balance and track processing status.",
  },
];

export function getDashboardNavItem(pathname: string): DashboardNavItem | undefined {
  return dashboardNavigationItems.find((item) => {
    if (item.href === "/dashboard") {
      return pathname === "/dashboard";
    }

    return pathname === item.href || pathname.startsWith(`${item.href}/`);
  });
}
