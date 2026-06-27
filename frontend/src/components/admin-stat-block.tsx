import { cn } from "@/lib/utils";

interface AdminStatBlockProps {
  label: string;
  value: string;
  featured?: boolean;
  className?: string;
}

export function AdminStatBlock({
  label,
  value,
  featured = false,
  className,
}: AdminStatBlockProps) {
  return (
    <div
      className={cn(
        "rounded-lg bg-muted/30 px-3 py-2",
        featured && "border-l-[3px] border-l-brand bg-brand-muted",
        className,
      )}
    >
      <p className="text-xs text-muted-foreground">{label}</p>
      <p
        className={cn(
          "mt-1 font-semibold tabular-nums tracking-tight text-foreground",
          featured ? "text-lg" : "text-base",
        )}
      >
        {value}
      </p>
    </div>
  );
}
