export function SiteFooter() {
  return (
    <footer className="border-t bg-background">
      <div className="mx-auto flex h-14 max-w-6xl items-center px-4 text-sm text-muted-foreground sm:px-6">
        <p>&copy; {new Date().getFullYear()} Cashback Affiliate Platform</p>
      </div>
    </footer>
  );
}
