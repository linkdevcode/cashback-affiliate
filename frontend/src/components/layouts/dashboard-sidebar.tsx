const navigationItems = [
  { label: "Dashboard" },
  { label: "Affiliate Links" },
  { label: "Orders" },
  { label: "Withdrawals" },
] as const;

export function DashboardSidebar() {
  return (
    <aside className="hidden w-64 shrink-0 border-r bg-sidebar md:block">
      <div className="flex h-16 items-center border-b px-6">
        <span className="text-base font-semibold">Cashback</span>
      </div>

      <nav className="space-y-1 p-4">
        {navigationItems.map((item) => (
          <div
            key={item.label}
            className="rounded-md px-3 py-2 text-sm text-muted-foreground"
          >
            {item.label}
          </div>
        ))}
      </nav>
    </aside>
  );
}
