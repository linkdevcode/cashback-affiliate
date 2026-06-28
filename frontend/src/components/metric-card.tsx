import {
  Card,
  CardContent,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { cn } from "@/lib/utils";

interface MetricCardProps {
  label: string;
  value: string;
  description: string;
  featured?: boolean;
  className?: string;
}

export function MetricCard({
  label,
  value,
  description,
  featured = false,
  className,
}: MetricCardProps) {
  return (
    <Card
      className={cn(
        featured && "border-l-[3px] border-l-brand bg-brand-muted",
        className,
      )}
    >
      <CardContent className="space-y-1 pt-6">
        <p className="text-xs font-medium text-muted-foreground">{label}</p>
        <p
          className={cn(
            "font-semibold tabular-nums tracking-tight text-foreground",
            featured ? "text-[1.75rem] leading-8" : "text-2xl leading-7",
          )}
        >
          {value}
        </p>
        <p className="text-xs text-muted-foreground">{description}</p>
      </CardContent>
    </Card>
  );
}

export function MetricCardSkeleton({ featured = false }: { featured?: boolean }) {
  return (
    <Card
      className={cn(
        featured && "border-l-[3px] border-l-brand bg-brand-muted",
      )}
    >
      <CardContent className="space-y-3 pt-6">
        <Skeleton className="h-3 w-24" />
        <Skeleton className="h-8 w-32" />
        <Skeleton className="h-3 w-28" />
      </CardContent>
    </Card>
  );
}
