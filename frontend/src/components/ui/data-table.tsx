import type { ReactNode } from "react"

import { cn } from "@/lib/utils"

interface DataTableProps extends React.ComponentPropsWithoutRef<"table"> {
  wrapperClassName?: string
  caption?: string
  children: ReactNode
}

export function DataTable({
  wrapperClassName,
  caption,
  className,
  children,
  ...props
}: DataTableProps) {
  return (
    <div
      className={cn(
        "overflow-x-auto rounded-3xl border border-border bg-card shadow-sm",
        wrapperClassName,
      )}
    >
      <table className={cn("min-w-full border-collapse px-4 text-sm text-left text-foreground", className)} {...props}>
        {caption ? <caption className="sr-only">{caption}</caption> : null}
        {children}
      </table>
    </div>
  )
}

export function DataTableHeader({ className, ...props }: React.ComponentPropsWithoutRef<"thead">) {
  return (
    <thead
      className={cn(
        "border-b border-border bg-muted/50 text-xs uppercase tracking-[0.12em] text-muted-foreground",
        className,
      )}
      {...props}
    />
  )
}

export function DataTableBody({ className, ...props }: React.ComponentPropsWithoutRef<"tbody">) {
  return <tbody className={cn(className)} {...props} />
}

export function DataTableRow({ className, ...props }: React.ComponentPropsWithoutRef<"tr">) {
  return (
    <tr className={cn("border-b border-border last:border-b-0 hover:bg-muted/30 transition-colors", className)} {...props} />
  )
}

export function DataTableCell({ className, ...props }: React.ComponentPropsWithoutRef<"td">) {
  return <td className={cn("px-4 py-3 align-top", className)} {...props} />
}

export function DataTableHeaderCell({ className, ...props }: React.ComponentPropsWithoutRef<"th">) {
  return <th className={cn("px-4 py-3 text-left font-semibold", className)} {...props} />
}
