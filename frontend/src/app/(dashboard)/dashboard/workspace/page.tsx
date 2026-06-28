"use client";

import Link from "next/link";

import { LinkConverterForm } from "@/features/affiliate-links/components/link-converter-form";
import { useDashboardSummary } from "@/features/dashboard/hooks/use-dashboard-summary";
import { DashboardSummaryCards } from "@/features/dashboard/components/dashboard-summary-cards";
import { EarningsChart } from "@/features/dashboard/components/earnings-chart";
import { RecentOrdersWidget } from "@/features/dashboard/components/recent-orders-widget";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { emptyDashboardSummary } from "@/types/dashboard";

export default function WorkspacePage() {
  const { data, isLoading, isError, error } = useDashboardSummary();
  const summary = data ?? emptyDashboardSummary;

  return (
    <div className="mx-auto max-w-7xl space-y-10 px-4 py-6 sm:px-6 lg:px-8">
      <div className="space-y-3">
        <div className="flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
          <div className="space-y-2">
            <p className="text-sm font-semibold uppercase tracking-[0.24em] text-muted-foreground">
              Không gian hoàn tiền
            </p>
            <h1 className="text-3xl font-semibold tracking-tight text-foreground">
              Bảng làm việc hoàn tiền
            </h1>
            <p className="max-w-2xl text-sm text-muted-foreground">
              Chuyển đổi link Shopee, theo dõi số dư ví và xem các đơn hoàn tiền gần đây tại một nơi.
            </p>
          </div>
          <Link
            href="/dashboard"
            className="text-sm font-medium text-primary hover:text-primary/80"
          >
            Quay lại bảng điều khiển
          </Link>
        </div>
      </div>

      <div className="grid grid-cols-1 gap-6 xl:grid-cols-[1.45fr_1fr]">
        <section className="space-y-4">
          <div className="rounded-3xl border border-border bg-card p-6 shadow-sm">
            <div className="space-y-4">
              <div>
                <p className="text-sm font-semibold uppercase tracking-[0.24em] text-muted-foreground">
                  Bộ chuyển đổi link
                </p>
                <h2 className="mt-2 text-2xl font-semibold tracking-tight text-foreground">
                  Tạo liên kết hoàn tiền đối tác
                </h2>
                <p className="mt-2 text-sm text-muted-foreground">
                  Dán đường dẫn sản phẩm Shopee để tạo ngay liên kết hoàn tiền đã kích hoạt.
                </p>
              </div>
              <LinkConverterForm />
            </div>
          </div>
        </section>

        <section className="space-y-4">
          <div className="rounded-3xl border border-border bg-card p-6 shadow-sm">
            <div>
              <p className="text-sm font-semibold uppercase tracking-[0.24em] text-muted-foreground">
                Tóm tắt ví
              </p>
              <h2 className="mt-2 text-2xl font-semibold tracking-tight text-foreground">
                Số dư của bạn trong một cái nhìn
              </h2>
              <p className="mt-2 text-sm text-muted-foreground">
                Xem số dư khả dụng, hoàn tiền đang chờ, tổng thu nhập và số tiền đã rút.
              </p>
            </div>
          </div>

          <DashboardSummaryCards
            summary={summary}
            isLoading={isLoading}
            isError={isError}
            error={error}
          />
        </section>
      </div>

      <div className="grid grid-cols-1 gap-6 xl:grid-cols-[2fr_1fr]">
        <section className="space-y-4">
          <div className="rounded-3xl border border-border bg-card p-6 shadow-sm">
            <div>
              <p className="text-sm font-semibold uppercase tracking-[0.24em] text-muted-foreground">
                Tổng quan thu nhập
              </p>
              <h2 className="mt-2 text-2xl font-semibold tracking-tight text-foreground">
                Xu hướng hoàn tiền theo tháng
              </h2>
              <p className="mt-2 text-sm text-muted-foreground">
                Theo dõi hiệu suất hoàn tiền của bạn trong 6 tháng gần nhất.</p>
            </div>
          </div>

          <EarningsChart
            cashbackByMonth={summary.cashbackByMonth}
            isLoading={isLoading}
          />
        </section>

        <RecentOrdersWidget orders={summary.recentOrders} isLoading={isLoading} />
      </div>
    </div>
  );
}
