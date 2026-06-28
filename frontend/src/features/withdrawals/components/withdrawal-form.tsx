"use client";

import { useQueryClient } from "@tanstack/react-query";
import { CheckCircle2, Clock, Loader2, ShieldCheck } from "lucide-react";
import { useState } from "react";

import { MetricCard, MetricCardSkeleton } from "@/components/metric-card";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import {
  dashboardSummaryQueryKey,
  useDashboardSummary,
} from "@/features/dashboard/hooks/use-dashboard-summary";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
import { useCreateWithdrawal } from "@/features/withdrawals/hooks/use-create-withdrawal";
import { withdrawalsQueryKey } from "@/features/withdrawals/hooks/use-withdrawals";
import { isApiError } from "@/services/api-client";
import { emptyDashboardSummary } from "@/types/dashboard";
import { MIN_WITHDRAWAL_AMOUNT } from "@/types/withdrawal";

interface WithdrawalFormState {
  amount: string;
  bankName: string;
  bankAccountNumber: string;
  bankAccountHolder: string;
}

const initialFormState: WithdrawalFormState = {
  amount: "",
  bankName: "",
  bankAccountNumber: "",
  bankAccountHolder: "",
};

export function WithdrawalForm() {
  const [form, setForm] = useState<WithdrawalFormState>(initialFormState);
  const [clientError, setClientError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const queryClient = useQueryClient();
  const { data: dashboardSummary, isLoading: isBalanceLoading } = useDashboardSummary();
  const createWithdrawal = useCreateWithdrawal();

  const availableBalance =
    dashboardSummary?.availableBalance ?? emptyDashboardSummary.availableBalance;
  const canWithdraw = availableBalance >= MIN_WITHDRAWAL_AMOUNT;

  const updateField = (field: keyof WithdrawalFormState, value: string) => {
    setForm((current) => ({ ...current, [field]: value }));
    setClientError(null);
    setSuccessMessage(null);
  };

  const handleUseMaxAmount = () => {
    if (availableBalance >= MIN_WITHDRAWAL_AMOUNT) {
      updateField("amount", String(availableBalance));
    }
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setClientError(null);
    setSuccessMessage(null);
    createWithdrawal.reset();

    const amount = Number(form.amount);

    if (!Number.isFinite(amount) || amount <= 0) {
      setClientError("Withdrawal amount must be greater than zero.");
      return;
    }

    if (amount < MIN_WITHDRAWAL_AMOUNT) {
      setClientError(
        `Minimum withdrawal amount is ${formatCurrency(MIN_WITHDRAWAL_AMOUNT)}.`,
      );
      return;
    }

    if (amount > availableBalance) {
      setClientError("Withdrawal amount cannot exceed your available balance.");
      return;
    }

    const bankName = form.bankName.trim();
    const bankAccountNumber = form.bankAccountNumber.trim();
    const bankAccountHolder = form.bankAccountHolder.trim();

    if (!bankName || !bankAccountNumber || !bankAccountHolder) {
      setClientError("All bank details are required.");
      return;
    }

    try {
      await createWithdrawal.mutateAsync({
        amount,
        bankName,
        bankAccountNumber,
        bankAccountName: bankAccountHolder,
      });

      setForm(initialFormState);
      setSuccessMessage("Withdrawal request submitted. Your balance has been reserved.");

      await Promise.all([
        queryClient.invalidateQueries({ queryKey: withdrawalsQueryKey }),
        queryClient.invalidateQueries({ queryKey: dashboardSummaryQueryKey }),
      ]);
    } catch {
      // Error rendered from mutation state.
    }
  };

  return (
    <div className="space-y-6">
      <div className="grid gap-6 lg:grid-cols-3">
        <div className="lg:col-span-1">
          {isBalanceLoading ? (
            <MetricCardSkeleton featured />
          ) : (
            <div className="rounded-2xl border border-border bg-card p-6 shadow-sm">
              <p className="text-xs font-medium text-muted-foreground">Available balance</p>
              <div className="mt-2 flex items-baseline justify-between">
                <p className="text-3xl font-semibold tabular-nums">{formatCurrency(availableBalance)}</p>
                <button
                  type="button"
                  onClick={handleUseMaxAmount}
                  disabled={!canWithdraw}
                  className="rounded-full border border-border px-3 py-1 text-sm text-muted-foreground hover:bg-muted/30"
                >
                  Use max
                </button>
              </div>
              <p className="mt-2 text-xs text-muted-foreground">Ready to transfer · Reserved when requested</p>
            </div>
          )}

          <Card className="mt-4">
            <CardContent className="space-y-4 pt-6">
              <div className="flex gap-3">
                <ShieldCheck className="mt-0.5 size-4 shrink-0 text-muted-foreground" />
                <div className="space-y-1">
                  <p className="text-sm font-medium">Trusted transfers</p>
                  <p className="text-xs text-muted-foreground">
                    Withdrawals are protected and reviewed prior to bank transfer.
                  </p>
                </div>
              </div>
              <div className="flex gap-3">
                <Clock className="mt-0.5 size-4 shrink-0 text-muted-foreground" />
                <div className="space-y-1">
                  <p className="text-sm font-medium">Typical processing</p>
                  <p className="text-xs text-muted-foreground">1–3 business days after approval</p>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>

        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle>Withdraw funds</CardTitle>
            <CardDescription>Bank transfer — secure, auditable, and compliant.</CardDescription>
          </CardHeader>

          <CardContent className="space-y-6">
            {!isBalanceLoading && !canWithdraw ? (
              <div className="rounded-lg bg-warning-muted px-3 py-2 text-xs text-warning" role="status">
                Minimum withdrawal is {formatCurrency(MIN_WITHDRAWAL_AMOUNT)}. Earn more cashback to request a payout.
              </div>
            ) : null}

            <form className="space-y-6" onSubmit={handleSubmit}>
              <div className="space-y-4">
                <p className="text-sm font-medium">Amount to withdraw</p>
                <div className="flex gap-3 items-center">
                  <Input
                    id="withdrawal-amount"
                    name="amount"
                    type="number"
                    min={MIN_WITHDRAWAL_AMOUNT}
                    step={1000}
                    inputMode="numeric"
                    placeholder={`Minimum ${MIN_WITHDRAWAL_AMOUNT.toLocaleString("vi-VN")}`}
                    value={form.amount}
                    onChange={(event) => updateField("amount", event.target.value)}
                    disabled={createWithdrawal.isPending || isBalanceLoading || !canWithdraw}
                    required
                    className="tabular-nums text-lg"
                  />
                  <div className="flex flex-col gap-2">
                    <Button
                      type="button"
                      variant="ghost"
                      size="sm"
                      onClick={handleUseMaxAmount}
                      disabled={!canWithdraw}
                    >
                      Withdraw max
                    </Button>
                  </div>
                </div>
                <p className="text-xs text-muted-foreground">Minimum: {formatCurrency(MIN_WITHDRAWAL_AMOUNT)} · Balance reserved on submit</p>
              </div>

              <div className="space-y-4">
                <p className="text-sm font-medium">Bank account</p>
                <div className="grid gap-4 sm:grid-cols-2">
                  <div className="space-y-1.5 sm:col-span-2">
                    <label htmlFor="bank-name" className="text-xs font-medium text-muted-foreground">Bank name</label>
                    <Input
                      id="bank-name"
                      name="bankName"
                      autoComplete="organization"
                      placeholder="Vietcombank"
                      value={form.bankName}
                      onChange={(event) => updateField("bankName", event.target.value)}
                      disabled={createWithdrawal.isPending || !canWithdraw}
                      required
                    />
                  </div>

                  <div className="space-y-1.5">
                    <label htmlFor="account-number" className="text-xs font-medium text-muted-foreground">Account number</label>
                    <Input
                      id="account-number"
                      name="bankAccountNumber"
                      autoComplete="off"
                      inputMode="numeric"
                      placeholder="0123456789"
                      value={form.bankAccountNumber}
                      onChange={(event) => updateField("bankAccountNumber", event.target.value)}
                      disabled={createWithdrawal.isPending || !canWithdraw}
                      required
                      className="font-mono"
                    />
                  </div>

                  <div className="space-y-1.5">
                    <label htmlFor="account-holder" className="text-xs font-medium text-muted-foreground">Account holder</label>
                    <Input
                      id="account-holder"
                      name="bankAccountHolder"
                      autoComplete="name"
                      placeholder="Nguyen Van A"
                      value={form.bankAccountHolder}
                      onChange={(event) => updateField("bankAccountHolder", event.target.value)}
                      disabled={createWithdrawal.isPending || !canWithdraw}
                      required
                    />
                    <p className="text-xs text-muted-foreground">Must match your bank account name exactly</p>
                  </div>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <Button
                  type="submit"
                  className="w-full sm:w-auto"
                  disabled={createWithdrawal.isPending || isBalanceLoading || !canWithdraw}
                >
                  {createWithdrawal.isPending ? (
                    <>
                      <Loader2 className="animate-spin" />
                      Submitting...
                    </>
                  ) : (
                    "Request payout"
                  )}
                </Button>

                <div className="text-sm text-muted-foreground">
                  <p>Secure transfer · Review in 24h</p>
                </div>
              </div>

              {clientError ? <p className="text-sm text-destructive" role="alert">{clientError}</p> : null}

              {createWithdrawal.isError ? (
                <p className="text-sm text-destructive" role="alert">{getErrorMessage(createWithdrawal.error)}</p>
              ) : null}

              {successMessage ? (
                <p className="flex items-center gap-2 rounded-lg bg-success-muted px-3 py-2 text-sm text-success" role="status">
                  <CheckCircle2 className="size-4 shrink-0" />
                  {successMessage}
                </p>
              ) : null}
            </form>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}

function getErrorMessage(error: unknown): string {
  if (isApiError(error)) {
    const apiError = error.response?.data;

    if (apiError?.errors?.length) {
      return apiError.errors.join(" ");
    }

    if (apiError?.message) {
      return apiError.message;
    }
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "Unable to submit withdrawal request. Please try again.";
}

export function WithdrawalFormSkeleton() {
  return (
    <div className="grid gap-6 lg:grid-cols-3">
      <MetricCardSkeleton featured />
      <Card className="lg:col-span-2">
        <CardHeader>
          <Skeleton className="h-5 w-40" />
          <Skeleton className="h-4 w-64" />
        </CardHeader>
        <CardContent className="space-y-4">
          <Skeleton className="h-10 w-full" />
          <Skeleton className="h-10 w-full" />
          <Skeleton className="h-10 w-full" />
        </CardContent>
      </Card>
    </div>
  );
}
