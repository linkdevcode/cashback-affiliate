import { cn } from "@/lib/utils"
import type { ReactNode } from "react"

export type StatusBadgeVariant =
  | "success"
  | "pending"
  | "info"
  | "warning"
  | "destructive"
  | "neutral"

const variantClasses: Record<StatusBadgeVariant, string> = {
  success: "bg-success-muted text-success",
  pending: "bg-warning-muted text-warning",
  info: "bg-info-muted text-info",
  warning: "bg-warning-muted text-warning",
  destructive: "bg-destructive-muted text-destructive",
  neutral: "bg-muted text-muted-foreground",
}

interface StatusBadgeProps {
  label: string
  variant?: StatusBadgeVariant
  icon?: ReactNode
  className?: string
}

export function StatusBadge({
  label,
  variant = "neutral",
  icon,
  className,
}: StatusBadgeProps) {
  return (
    <span
      className={cn(
        "inline-flex items-center gap-2 rounded-full px-2.5 py-1 text-xs font-semibold",
        variantClasses[variant],
        className,
      )}
    >
      {icon}
      {label}
    </span>
  )
}
