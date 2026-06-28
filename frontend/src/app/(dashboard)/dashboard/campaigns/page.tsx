"use client";

import { useMemo, useState } from "react";
import { Search, TrendingUp, ShoppingBag, Tag } from "lucide-react";

import { useOrders } from "@/features/orders/hooks/use-orders";
import { EmptyState } from "@/components/empty-state";
import { Button, buttonVariants } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import type { OrderListItem } from "@/types/order";

const categories = [
  "All",
  "Shopee Mall",
  "Fashion",
  "Electronics",
  "Beauty",
  "Food",
];

const merchantCategoryMap: Record<string, string> = {
  shopeedelivery: "Shopee Mall",
  shopee: "Shopee Mall",
  mall: "Shopee Mall",
  fashion: "Fashion",
  beauty: "Beauty",
  electronics: "Electronics",
  phone: "Electronics",
  gadget: "Electronics",
  food: "Food",
  grocery: "Food",
  travel: "Travel",
};

function getMerchantCategory(merchant?: string | null) {
  const name = merchant?.toLowerCase() ?? "unknown";

  for (const key of Object.keys(merchantCategoryMap)) {
    if (name.includes(key)) {
      return merchantCategoryMap[key];
    }
  }

  return "All";
}

function getCashbackRate(order: OrderListItem) {
  if (!order.orderAmount || order.orderAmount <= 0) {
    return 0;
  }

  return (order.cashbackAmount / order.orderAmount) * 100;
}

function formatRate(value: number) {
  return `${value.toFixed(1)}%`;
}

export default function CampaignsPage() {
  const [searchQuery, setSearchQuery] = useState("");
  const [activeCategory, setActiveCategory] = useState("All");

  const { data, isLoading, isError, error } = useOrders({ pageSize: 50 });
  const orders = data?.items ?? [];

  const campaigns = useMemo(() => {
    const merchantMap = new Map<string, {
      merchant: string;
      category: string;
      totalCashback: number;
      totalOrders: number;
      averageRate: number;
      lastOrderDate: string;
    }>();

    for (const order of orders) {
      const merchantName = order.merchant?.trim() || "Unknown merchant";
      const normalizedMerchant = merchantName.toLowerCase();
      const category = getMerchantCategory(merchantName);
      const existing = merchantMap.get(normalizedMerchant);
      const cashbackRate = getCashbackRate(order);

      if (existing) {
        const totalOrders = existing.totalOrders + 1;
        const totalCashback = existing.totalCashback + order.cashbackAmount;
        const averageRate =
          (existing.averageRate * existing.totalOrders + cashbackRate) / totalOrders;
        const lastOrderDate =
          order.createdAt > existing.lastOrderDate
            ? order.createdAt
            : existing.lastOrderDate;

        merchantMap.set(normalizedMerchant, {
          merchant: merchantName,
          category,
          totalCashback,
          totalOrders,
          averageRate,
          lastOrderDate,
        });
      } else {
        merchantMap.set(normalizedMerchant, {
          merchant: merchantName,
          category,
          totalCashback: order.cashbackAmount,
          totalOrders: 1,
          averageRate: cashbackRate,
          lastOrderDate: order.createdAt,
        });
      }
    }

    return Array.from(merchantMap.values()).sort((a, b) => b.totalCashback - a.totalCashback);
  }, [orders]);

  const filteredCampaigns = useMemo(() => {
    const query = searchQuery.trim().toLowerCase();

    return campaigns.filter((campaign) => {
      const matchesCategory =
        activeCategory === "All" || campaign.category === activeCategory;
      const matchesSearch =
        !query || campaign.merchant.toLowerCase().includes(query);

      return matchesCategory && matchesSearch;
    });
  }, [campaigns, activeCategory, searchQuery]);

  const topCampaigns = filteredCampaigns.slice(0, 4);

  return (
    <div className="mx-auto max-w-7xl space-y-8 px-4 py-6 sm:px-6 lg:px-8">
      <div className="space-y-3">
        <div className="space-y-2">
          <p className="text-sm font-semibold uppercase tracking-[0.24em] text-muted-foreground">
            Tỷ lệ hoàn tiền
          </p>
          <h1 className="text-3xl font-semibold tracking-tight text-foreground">
            Chiến dịch đối tác
          </h1>
          <p className="max-w-2xl text-sm text-muted-foreground">
            Duyệt các ưu đãi hoàn tiền từ đối tác liên kết, so sánh tỷ lệ và lọc theo danh mục.
          </p>
        </div>
      </div>

      <div className="grid gap-6 xl:grid-cols-[1.25fr_0.75fr]">
        <section className="space-y-6">
          <Card>
            <CardHeader className="gap-4">
              <div>
                <CardTitle>Tìm chiến dịch</CardTitle>
                <CardDescription>
                  Tìm chiến dịch đối tác theo tên hoặc danh mục.
                </CardDescription>
              </div>
              <div className="relative max-w-md">
                <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  className="pl-10"
                  placeholder="Tìm đối tác…"
                  value={searchQuery}
                  onChange={(event) => setSearchQuery(event.target.value)}
                  aria-label="Tìm đối tác"
                />
              </div>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex flex-wrap gap-2">
                {categories.map((category) => {
                  const active = activeCategory === category;
                  return (
                    <button
                      key={category}
                      type="button"
                      onClick={() => setActiveCategory(category)}
                      className={buttonVariants({
                        variant: active ? "secondary" : "outline",
                        size: "sm",
                      })}
                    >
                      {category}
                    </button>
                  );
                })}
              </div>
            </CardContent>
          </Card>

          <section className="space-y-4">
            <div className="flex items-center justify-between gap-4">
              <div>
                <p className="text-sm font-semibold uppercase tracking-[0.24em] text-muted-foreground">
                  Thẻ đối tác
                </p>
                <h2 className="text-xl font-semibold tracking-tight text-foreground">
                  Top chiến dịch hoàn tiền
                </h2>
              </div>
              <span className="text-sm text-muted-foreground">
                {filteredCampaigns.length} đối tác tìm thấy
              </span>
            </div>

            {isLoading ? (
              <div className="grid gap-4 md:grid-cols-2">
                {Array.from({ length: 4 }).map((_, index) => (
                  <Skeleton key={index} className="h-40 rounded-[1.5rem]" />
                ))}
              </div>
            ) : null}

            {!isLoading && filteredCampaigns.length === 0 ? (
              <Card>
                <CardContent>
                  <EmptyState
                    title="Không tìm thấy chiến dịch"
                    description="Hãy thử từ khóa hoặc danh mục khác để khám phá các tỷ lệ hoàn tiền."
                    action={
                      <Button
                        type="button"
                        variant="outline"
                        onClick={() => {
                          setSearchQuery("");
                          setActiveCategory("All");
                        }}
                      >
                        Đặt lại bộ lọc
                      </Button>
                    }
                  />
                </CardContent>
              </Card>
            ) : null}

            <div className="grid gap-4 md:grid-cols-2">
              {!isLoading && topCampaigns.map((campaign) => (
                <Card key={campaign.merchant} className="overflow-hidden">
                  <CardContent className="space-y-4">
                    <div className="flex items-start justify-between gap-4">
                      <div>
                        <p className="text-sm font-semibold text-muted-foreground">
                          {campaign.category}
                        </p>
                        <h3 className="mt-1 text-lg font-semibold text-foreground">
                          {campaign.merchant}
                        </h3>
                      </div>
                      <div className="rounded-2xl bg-emerald-500/10 px-3 py-2 text-emerald-700">
                        <p className="text-xs uppercase tracking-[0.2em] text-emerald-700">
                          Tỷ lệ
                        </p>
                        <p className="mt-1 text-xl font-semibold">
                          {formatRate(campaign.averageRate)}
                        </p>
                      </div>
                    </div>

                    <div className="grid gap-3 sm:grid-cols-2">
                      <div className="rounded-2xl border border-border bg-muted p-4">
                        <p className="text-xs text-muted-foreground">Total cashback</p>
                        <p className="mt-2 text-sm font-semibold">
                          {formatCurrency(campaign.totalCashback)}
                        </p>
                      </div>
                      <div className="rounded-2xl border border-border bg-muted p-4">
                        <p className="text-xs text-muted-foreground">Orders</p>
                        <p className="mt-2 text-sm font-semibold">{campaign.totalOrders}</p>
                      </div>
                    </div>

                    <div className="flex items-center justify-between gap-2 text-sm text-muted-foreground">
                      <span>Last order</span>
                      <span>{new Date(campaign.lastOrderDate).toLocaleDateString("vi-VN")}</span>
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          </section>
        </section>

        <aside className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Cashback rates table</CardTitle>
              <CardDescription>
                Compare merchants and predicted average cashback percentages.
              </CardDescription>
            </CardHeader>
            <CardContent className="overflow-x-auto">
              <table className="min-w-full text-left text-sm">
                <thead className="border-b border-border bg-muted/40">
                  <tr>
                    <th className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Merchant
                    </th>
                    <th className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Category
                    </th>
                    <th className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Avg. rate
                    </th>
                    <th className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Cashback
                    </th>
                    <th className="px-4 py-3 text-xs font-medium text-muted-foreground">
                      Orders
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {isLoading ? (
                    Array.from({ length: 5 }).map((_, index) => (
                      <tr key={index} className="border-b border-border">
                        <td className="px-4 py-3"><Skeleton className="h-4 w-full" /></td>
                        <td className="px-4 py-3"><Skeleton className="h-4 w-full" /></td>
                        <td className="px-4 py-3"><Skeleton className="h-4 w-full" /></td>
                        <td className="px-4 py-3"><Skeleton className="h-4 w-full" /></td>
                        <td className="px-4 py-3"><Skeleton className="h-4 w-full" /></td>
                      </tr>
                    ))
                  ) : (
                    filteredCampaigns.map((campaign) => (
                      <tr key={campaign.merchant} className="border-b border-border hover:bg-muted/40">
                        <td className="px-4 py-3 font-medium">{campaign.merchant}</td>
                        <td className="px-4 py-3 text-muted-foreground">{campaign.category}</td>
                        <td className="px-4 py-3 font-semibold">
                          {formatRate(campaign.averageRate)}
                        </td>
                        <td className="px-4 py-3 tabular-nums">
                          {formatCurrency(campaign.totalCashback)}
                        </td>
                        <td className="px-4 py-3">{campaign.totalOrders}</td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </CardContent>
          </Card>

          <Card className="rounded-3xl border border-dashed border-border bg-muted p-6">
            <div className="flex items-start gap-4">
              <div className="rounded-2xl bg-emerald-500/10 p-3 text-emerald-700">
                <TrendingUp className="h-5 w-5" />
              </div>
              <div>
                <p className="text-sm font-semibold text-foreground">Bonus tip</p>
                <p className="mt-1 text-sm text-muted-foreground">
                  Focus on merchants with stable rates and recent orders to maximize cashback earnings.
                </p>
              </div>
            </div>
          </Card>
        </aside>
      </div>
    </div>
  );
}
