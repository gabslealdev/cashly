using Cashly.Application.TransactionContext.UseCases.RegisterTransaction;
using Shouldly;

namespace Cashly.Application.UnitTests.TransactionContext.RegisterTransaction;

public sealed class CreateTransactionCommandValidatorTests
{
    private readonly CreateTransactionCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldSucceed_WhenCommandIsValid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("UserId", "User is required")]
    [InlineData("CashflowId", "Cashflow is required")]
    [InlineData("Title", "Title is required")]
    [InlineData("Amount", "Amount is required")]
    [InlineData("Type", "Type is required")]
    [InlineData("Date", "Date is required")]
    [InlineData("Status", "Status is required")]
    public void Validate_ShouldFail_WhenRequiredFieldIsEmpty(
        string propertyName,
        string expectedMessage)
    {
        var command = CreateCommandWithEmptyProperty(propertyName);

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error =>
            error.PropertyName == propertyName &&
            error.ErrorMessage == expectedMessage);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTitleExceedsMaximumLength()
    {
        var command = CreateValidCommand() with { Title = new string('a', 51) };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(CreateTransactionCommand.Title) &&
            error.ErrorMessage == "Title must not exceed 50 characters");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    public void Validate_ShouldFail_WhenAmountIsNegative(decimal amount)
    {
        var command = CreateValidCommand() with { Amount = amount };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(CreateTransactionCommand.Amount) &&
            error.ErrorMessage == "Amount must be positive");
    }

    [Theory]
    [InlineData("Credit")]
    [InlineData("Unknown")]
    public void Validate_ShouldFail_WhenTransactionTypeIsInvalid(string type)
    {
        var command = CreateValidCommand() with { Type = type };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(CreateTransactionCommand.Type) &&
            error.ErrorMessage == "Type not supported");
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Unknown")]
    public void Validate_ShouldFail_WhenTransactionStatusIsInvalid(string status)
    {
        var command = CreateValidCommand() with { Status = status };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(CreateTransactionCommand.Status) &&
            error.ErrorMessage == "Status not supported");
    }

    private static CreateTransactionCommand CreateValidCommand() =>
        new(
            UserId: Guid.NewGuid(),
            CashflowId: Guid.NewGuid(),
            Title: "Mensalidade academia",
            Amount: 500m,
            Type: "Expense",
            Date: DateTimeOffset.UtcNow,
            Status: "Completed");

    private static CreateTransactionCommand CreateCommandWithEmptyProperty(string propertyName)
    {
        var command = CreateValidCommand();

        return propertyName switch
        {
            nameof(CreateTransactionCommand.UserId) => command with { UserId = Guid.Empty },
            nameof(CreateTransactionCommand.CashflowId) => command with { CashflowId = Guid.Empty },
            nameof(CreateTransactionCommand.Title) => command with { Title = string.Empty },
            nameof(CreateTransactionCommand.Amount) => command with { Amount = 0m },
            nameof(CreateTransactionCommand.Type) => command with { Type = string.Empty },
            nameof(CreateTransactionCommand.Date) => command with { Date = default },
            nameof(CreateTransactionCommand.Status) => command with { Status = string.Empty },
            _ => throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null)
        };
    }
}
