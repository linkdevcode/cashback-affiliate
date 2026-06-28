"use client";

import { Check, Circle, Loader2 } from "lucide-react";

import { OrderStatus } from "@/types/order";

interface OrderStatusTimelineProps {
  status: OrderStatus;
  statusName: string;
}

interface TimelineStep {
  label: string;
  completed: boolean;
  active: boolean;
}

export function OrderStatusTimeline({ status, statusName }: OrderStatusTimelineProps) {
  const steps = getTimelineSteps(status);

  return (
    <div className="space-y-3">
      <p className="text-xs font-medium text-muted-foreground">Status timeline</p>
      <ol className="space-y-3">
        {steps.map((step) => (
          <li key={step.label} className="flex items-start gap-3">
            <div className="mt-0.5">
              {step.completed ? (
                <span className="flex size-5 items-center justify-center rounded-full bg-primary text-primary-foreground">
                  <Check className="size-3" />
                </span>
              ) : step.active ? (
                <span className="flex size-5 items-center justify-center rounded-full border-2 border-primary">
                  <Loader2 className="size-3 animate-spin text-primary" />
                </span>
              ) : (
                <span className="flex size-5 items-center justify-center rounded-full border border-muted-foreground/30">
                  <Circle className="size-2.5 text-muted-foreground/50" />
                </span>
              )}
            </div>

            <div className="min-w-0 flex-1">
              <p
                className={`text-sm font-medium ${
                  step.active || step.completed ? "text-foreground" : "text-muted-foreground"
                }`}
              >
                {step.label}
              </p>
              {step.active ? (
                <p className="text-xs text-muted-foreground">Current status: {statusName}</p>
              ) : null}
            </div>
          </li>
        ))}
      </ol>
    </div>
  );
}

function getTimelineSteps(status: OrderStatus): TimelineStep[] {
  if (status === OrderStatus.Pending) {
    return [
      { label: "Order received", completed: true, active: false },
      { label: "Pending review", completed: false, active: true },
      { label: "Awaiting result", completed: false, active: false },
    ];
  }

  if (status === OrderStatus.Approved) {
    return [
      { label: "Order received", completed: true, active: false },
      { label: "Pending review", completed: true, active: false },
      { label: "Approved", completed: true, active: false },
    ];
  }

  return [
    { label: "Order received", completed: true, active: false },
    { label: "Pending review", completed: true, active: false },
    { label: "Rejected", completed: true, active: false },
  ];
}
