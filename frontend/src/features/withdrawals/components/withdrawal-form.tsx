"use client";

import { useQueryClient } from "@tanstack/react-query";
import { CheckCircle2, Loader2, Wallet } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import {
  dashboardSummaryQueryKey,
  useDashboardSummary,
} from "@/features/dashboard/hooks/use-dashboard-summary";
import { useCreateWithdrawal } from "@/features/withdrawals/hooks/use-create-withdrawal";
import { withdrawalsQueryKey } from "@/features/withdrawals/hooks/use-withdrawals";
import { formatCurrency } from "@/features/orders/lib/order-formatters";
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

  const availableBalance = dashboardSummary?.availableBalance ?? emptyDashboardSummary.availableBalance;

  const updateField = (field: keyof WithdrawalFormState, value: string) => {
    setForm((current) => ({ ...current, [field]: value }));
    setClientError(null);
    setSuccessMessage(null);
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
      setSuccessMessage("Withdrawal request submitted successfully.");

      await Promise.all([
        queryClient.invalidateQueries({ queryKey: withdrawalsQueryKey }),
        queryClient.invalidateQueries({ queryKey: dashboardSummaryQueryKey }),
      ]);
    } catch {
      // Error rendered from mutation state.
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Request withdrawal</CardTitle>
        <CardDescription>
          Transfer your available cashback balance to your bank account.
        </CardDescription>
      </CardHeader>

      <CardContent className="space-y-6">
        <div className="flex items-center justify-between rounded-lg border bg-muted/30 px-4 py-3">
          <div className="flex items-center gap-3">
            <div className="flex size-10 items-center justify-center rounded-full bg-primary/10 text-primary">
              <Wallet className="size-5" />
            </div>
            <div>
              <p className="text-sm text-muted-foreground">Available balance</p>
              {isBalanceLoading ? (
                <div className="mt-1 flex items-center gap-2 text-sm text-muted-foreground">
                  <Loader2 className="size-4 animate-spin" />
                  Loading...
                </div>
              ) : (
                <p className="text-xl font-semibold">{formatCurrency(availableBalance)}</p>
              )}
            </div>
          </div>
        </div>

        <form className="space-y-4" onSubmit={handleSubmit}>
          <div className="space-y-2">
            <label htmlFor="withdrawal-amount" className="text-sm font-medium">
              Amount (VND)
            </label>
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
              disabled={createWithdrawal.isPending || isBalanceLoading}
              required
            />
            <p className="text-xs text-muted-foreground">
              Minimum withdrawal: {formatCurrency(MIN_WITHDRAWAL_AMOUNT)}
            </p>
          </div>

          <div className="space-y-2">
            <label htmlFor="bank-name" className="text-sm font-medium">
              Bank name
            </label>
            <Input
              id="bank-name"
              name="bankName"
              autoComplete="organization"
              placeholder="Vietcombank"
              value={form.bankName}
              onChange={(event) => updateField("bankName", event.target.value)}
              disabled={createWithdrawal.isPending}
              required
            />
          </div>

          <div className="space-y-2">
            <label htmlFor="account-number" className="text-sm font-medium">
              Account number
            </label>
            <Input
              id="account-number"
              name="bankAccountNumber"
              autoComplete="off"
              inputMode="numeric"
              placeholder="0123456789"
              value={form.bankAccountNumber}
              onChange={(event) => updateField("bankAccountNumber", event.target.value)}
              disabled={createWithdrawal.isPending}
              required
            />
          </div>

          <div className="space-y-2">
            <label htmlFor="account-holder" className="text-sm font-medium">
              Account holder
            </label>
            <Input
              id="account-holder"
              name="bankAccountHolder"
              autoComplete="name"
              placeholder="Nguyen Van A"
              value={form.bankAccountHolder}
              onChange={(event) => updateField("bankAccountHolder", event.target.value)}
              disabled={createWithdrawal.isPending}
              required
            />
          </div>

          <Button
            type="submit"
            className="w-full"
            disabled={createWithdrawal.isPending || isBalanceLoading}
          >
            {createWithdrawal.isPending ? (
              <>
                <Loader2 className="animate-spin" />
                Submitting...
              </>
            ) : (
              "Submit withdrawal request"
            )}
          </Button>

          {clientError ? (
            <p className="text-sm text-destructive" role="alert">
              {clientError}
            </p>
          ) : null}

          {createWithdrawal.isError ? (
            <p className="text-sm text-destructive" role="alert">
              {getErrorMessage(createWithdrawal.error)}
            </p>
          ) : null}

          {successMessage ? (
            <p
              className="flex items-center gap-2 text-sm text-emerald-700 dark:text-emerald-400"
              role="status"
            >
              <CheckCircle2 className="size-4" />
              {successMessage}
            </p>
          ) : null}
        </form>
      </CardContent>
    </Card>
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
