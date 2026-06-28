export interface AdminNavItem {
  label: string;
  href: string;
  title: string;
  description: string;
}

export const adminNavigationItems: AdminNavItem[] = [
  {
    label: "Bảng điều khiển",
    href: "/admin",
    title: "Bảng điều khiển quản trị",
    description: "Tổng quan nền tảng với thống kê người dùng, đơn hàng, rút tiền và doanh thu.",
  },
  {
    label: "Người dùng",
    href: "/admin/users",
    title: "Quản lý người dùng",
    description: "Tìm kiếm người dùng, xem chi tiết tài khoản và quản lý trạng thái hoạt động hoặc tạm khóa.",
  },
  {
    label: "Đơn hàng",
    href: "/admin/orders",
    title: "Quản lý đơn hàng",
    description: "Tìm kiếm và lọc đơn hàng trên nền tảng rồi xem chi tiết chuyển đổi cho từng người dùng.",
  },
  {
    label: "Yêu cầu rút tiền",
    href: "/admin/withdrawals",
    title: "Quản lý rút tiền",
    description: "Xem các yêu cầu rút tiền, lọc theo trạng thái và phê duyệt hoặc từ chối thanh toán.",
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
