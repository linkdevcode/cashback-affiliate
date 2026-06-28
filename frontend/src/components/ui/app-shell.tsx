import type { ReactNode } from "react"

import { cn } from "@/lib/utils"

interface AppShellProps {
  topbar?: ReactNode
  sidebar?: ReactNode
  footer?: ReactNode
  children: ReactNode
  className?: string
  contentClassName?: string
}

export function AppShell({
  topbar,
  sidebar,
  footer,
  children,
  className,
  contentClassName,
}: AppShellProps) {
  return (
    <div className={cn("min-h-screen bg-background text-foreground", className)}>
      <div className="flex min-h-screen">
        {sidebar ? (
          <aside className="hidden w-72 shrink-0 border-r border-border bg-sidebar md:block">
            {sidebar}
          </aside>
        ) : null}

        <div className="flex min-h-screen flex-1 flex-col">
          {topbar ? (
            <div className="border-b border-border bg-background">
              {topbar}
            </div>
          ) : null}

          <main className={cn("flex-1 p-4 sm:p-6", contentClassName)}>{children}</main>

          {footer ? (
            <footer className="border-t border-border bg-background px-4 py-4 sm:px-6">
              {footer}
            </footer>
          ) : null}
        </div>
      </div>
    </div>
  )
}
