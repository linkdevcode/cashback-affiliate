import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "./card"
import { cn } from "@/lib/utils"
import type { ReactNode } from "react"

interface StatCardProps {
  title: string
  value: string | number
  description?: string
  icon?: ReactNode
  variant?: "default" | "success" | "warning" | "info"
  className?: string
}

const variantClasses: Record<NonNullable<StatCardProps["variant"]>, string> = {
  default: "bg-card text-card-foreground",
  success: "bg-success-muted text-success",
  warning: "bg-warning-muted text-warning",
  info: "bg-info-muted text-info",
}

export function StatCard({
  title,
  value,
  description,
  icon,
  variant = "default",
  className,
}: StatCardProps) {
  return (
    <Card className={cn("rounded-3xl border border-border shadow-sm", variantClasses[variant], className)}>
      <CardHeader>
        <div className="flex items-start justify-between gap-3">
          <div>
            <CardTitle className="text-base">{title}</CardTitle>
            {description ? (
              <CardDescription>{description}</CardDescription>
            ) : null}
          </div>
          {icon ? <div className="text-muted-foreground">{icon}</div> : null}
        </div>
      </CardHeader>
      <CardContent className="px-0 pt-3">
        <div className="text-3xl font-semibold tracking-tight">{value}</div>
      </CardContent>
    </Card>
  )
}
