import type { MonthlyCashback } from "@/types/dashboard";

export function formatMonthLabel(year: number, month: number): string {
  return new Intl.DateTimeFormat("vi-VN", {
    month: "short",
    year: "2-digit",
  }).format(new Date(year, month - 1, 1));
}

export function toChartData(monthlyCashback: MonthlyCashback[]) {
  return monthlyCashback.map((item) => ({
    month: formatMonthLabel(item.year, item.month),
    cashback: item.cashbackAmount,
    label: formatMonthLabel(item.year, item.month),
  }));
}
