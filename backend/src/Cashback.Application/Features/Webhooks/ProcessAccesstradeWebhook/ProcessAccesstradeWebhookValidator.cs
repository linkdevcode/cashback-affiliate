using FluentValidation;

namespace Cashback.Application.Features.Webhooks.ProcessAccesstradeWebhook;

/// <summary>
/// Validator for Accesstrade webhook postback payloads.
/// </summary>
public sealed class ProcessAccesstradeWebhookValidator
    : AbstractValidator<ProcessAccesstradeWebhookCommand>
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "pending",
        "approved",
        "rejected",
        "cancelled"
    };

    /// <summary>
    /// Initializes validation rules for Accesstrade webhook payloads.
    /// </summary>
    public ProcessAccesstradeWebhookValidator()
    {
        RuleFor(command => command.Payload)
            .NotNull()
            .WithMessage("Webhook payload is required.");

        RuleFor(command => command.Payload.OrderId)
            .NotEmpty()
            .WithMessage("Order ID is required.");

        RuleFor(command => command.Payload.Sub1)
            .NotEmpty()
            .WithMessage("Sub1 is required.");

        RuleFor(command => command.Payload.Commission)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Commission must be zero or greater.");

        RuleFor(command => command.Payload.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(BeAllowedStatus)
            .WithMessage("Status must be one of: pending, approved, rejected, cancelled.");
    }

    /// <summary>
    /// Validates that the status value is supported by the webhook contract.
    /// </summary>
    private static bool BeAllowedStatus(string status)
    {
        return AllowedStatuses.Contains(status.Trim());
    }
}
