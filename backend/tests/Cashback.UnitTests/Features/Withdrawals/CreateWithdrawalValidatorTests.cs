using Cashback.Application.Features.Withdrawals.CreateWithdrawal;
using Cashback.Application.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace Cashback.UnitTests.Features.Withdrawals;

/// <summary>
/// Unit tests for withdrawal request validation.
/// </summary>
public sealed class CreateWithdrawalValidatorTests
{
    /// <summary>
    /// Verifies valid withdrawal requests pass validation.
    /// </summary>
    [Fact]
    public void Validate_ValidRequest_ShouldNotHaveErrors()
    {
        var validator = CreateValidator(50_000m);
        var command = new CreateWithdrawalCommand(
            100_000m,
            "Vietcombank",
            "0123456789",
            "Nguyen Van A");

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Verifies zero amount is rejected.
    /// </summary>
    [Fact]
    public void Validate_ZeroAmount_ShouldHaveError()
    {
        var validator = CreateValidator(50_000m);
        var command = new CreateWithdrawalCommand(
            0m,
            "Vietcombank",
            "0123456789",
            "Nguyen Van A");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    /// <summary>
    /// Verifies amounts below the minimum are rejected.
    /// </summary>
    [Fact]
    public void Validate_BelowMinimumAmount_ShouldHaveError()
    {
        var validator = CreateValidator(50_000m);
        var command = new CreateWithdrawalCommand(
            49_999m,
            "Vietcombank",
            "0123456789",
            "Nguyen Van A");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    /// <summary>
    /// Verifies required bank fields are validated.
    /// </summary>
    [Fact]
    public void Validate_MissingBankFields_ShouldHaveErrors()
    {
        var validator = CreateValidator(50_000m);
        var command = new CreateWithdrawalCommand(
            100_000m,
            "",
            "",
            "");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BankName);
        result.ShouldHaveValidationErrorFor(x => x.BankAccountNumber);
        result.ShouldHaveValidationErrorFor(x => x.BankAccountHolder);
    }

    /// <summary>
    /// Creates a validator with the configured minimum withdrawal amount.
    /// </summary>
    private static CreateWithdrawalValidator CreateValidator(decimal minimumAmount)
    {
        var settings = new Mock<IWithdrawalSettings>();
        settings.SetupGet(x => x.MinimumWithdrawalAmount).Returns(minimumAmount);

        return new CreateWithdrawalValidator(settings.Object);
    }
}
