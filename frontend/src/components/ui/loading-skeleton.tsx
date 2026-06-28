import { cn } from "@/lib/utils"

interface LoadingSkeletonProps {
  className?: string
  variant?: "card" | "line" | "table" | "circle"
}

const variantClasses: Record<NonNullable<LoadingSkeletonProps["variant"]>, string> = {
  card: "h-48 rounded-3xl bg-muted/40",
  line: "h-4 rounded-full bg-muted/40",
  table: "min-w-full space-y-3 rounded-3xl bg-muted/40 p-4",
  circle: "h-12 w-12 rounded-full bg-muted/40",
}

export function LoadingSkeleton({ className, variant = "card" }: LoadingSkeletonProps) {
  return <div className={cn(variantClasses[variant], className)} />
}
