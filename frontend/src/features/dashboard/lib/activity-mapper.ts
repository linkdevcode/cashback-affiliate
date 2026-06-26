import type { OrderListItem } from "@/types/order";

export type DashboardActivityType = "order" | "status" | "cashback";

export interface DashboardActivity {
  id: string;
  type: DashboardActivityType;
  title: string;
  description: string;
  timestamp: string;
  amount?: number;
}

export function mapOrdersToActivities(orders: OrderListItem[]): DashboardActivity[] {
  return orders.flatMap((order) => [
    {
      id: `${order.id}-order`,
      type: "order",
      title: "Recent order",
      description: `Order ${order.orderCode} received`,
      timestamp: order.createdAt,
    },
    {
      id: `${order.id}-status`,
      type: "status",
      title: "Status change",
      description: `Order ${order.orderCode} is ${order.statusName}`,
      timestamp: order.createdAt,
    },
    {
      id: `${order.id}-cashback`,
      type: "cashback",
      title: "Cashback update",
      description: `Cashback recorded for order ${order.orderCode}`,
      timestamp: order.createdAt,
      amount: order.cashbackAmount,
    },
  ]);
}
