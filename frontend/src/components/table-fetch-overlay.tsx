import type { ReactNode } from "react";

import { cn } from "@/lib/utils";

interface TableFetchOverlayProps {
  isFetching: boolean;
  children: ReactNode;
  className?: string;
}

export function TableFetchOverlay({
  isFetching,
  children,
  className,
}: TableFetchOverlayProps) {
  return (
    <div className={cn("relative", className)}>
      {isFetching ? (
        <div
          className="absolute inset-x-0 top-0 z-10 h-0.5 overflow-hidden rounded-full bg-muted"
          aria-hidden="true"
        >
          <div className="h-full w-1/3 animate-pulse bg-brand" />
        </div>
      ) : null}

      <div
        className={cn(
          "transition-opacity",
          isFetching && "pointer-events-none opacity-60",
        )}
      >
        {children}
      </div>

      {isFetching ? (
        <p className="mt-2 text-xs text-muted-foreground">Updating…</p>
      ) : null}
    </div>
  );
}
