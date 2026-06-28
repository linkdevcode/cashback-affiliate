export interface DashboardNavItem {
  label: string;
  href: string;
  title: string;
  description?: string;
}

export const dashboardNavigationItems: DashboardNavItem[] = [
  {
    label: "Không gian hoàn tiền",
    href: "/dashboard/workspace",
    title: "Không gian hoàn tiền",
    description: "Chuyển đổi link, theo dõi số dư ví và giám sát hoạt động hoàn tiền gần đây.",
  },
  {
    label: "Tỷ lệ hoàn tiền",
    href: "/dashboard/campaigns",
    title: "Chiến dịch",
    description: "Duyệt các chiến dịch hoàn tiền từ đối tác, so sánh tỷ lệ và lọc theo danh mục.",
  },
  {
    label: "Liên kết đối tác",
    href: "/dashboard/affiliate",
    title: "Bộ chuyển đổi link",
    description: "Dán đường dẫn sản phẩm Shopee để tạo liên kết hoàn tiền của bạn.",
  },
  {
    label: "Đơn hàng",
    href: "/dashboard/orders",
    title: "Đơn hàng",
    description: "Xem lịch sử đơn hàng, theo dõi khoản hoàn tiền và trạng thái chuyển đổi.",
  },
  {
    label: "Yêu cầu rút tiền",
    href: "/dashboard/withdrawals",
    title: "Rút tiền",
    description: "Yêu cầu rút từ số dư khả dụng và theo dõi trạng thái xử lý.",
  },
];

export function getDashboardNavItem(pathname: string): DashboardNavItem | undefined {
  const effectivePath = pathname === "/dashboard" ? "/dashboard/workspace" : pathname;
  return dashboardNavigationItems.find(
    (item) =>
      effectivePath === item.href || effectivePath.startsWith(`${item.href}/`),
  );
}
