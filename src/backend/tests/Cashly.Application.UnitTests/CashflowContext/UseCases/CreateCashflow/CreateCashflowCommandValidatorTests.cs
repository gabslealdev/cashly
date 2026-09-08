using Cashly.Application.CashflowContext.UseCases.CreateCashflow;
using Shouldly;

namespace Cashly.Application.UnitTests.CashflowContext.UseCases.CreateCashflow;

public sealed class CreateCashflowCommandValidatorTests
{
    private readonly CreateCashflowCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldSucceed_WhenCommandIsValid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void Validate_ShouldFail_WhenTitleIsEmpty()
    {
        var command = CreateValidCommand() with { Title = string.Empty };

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(CreateCashflowCommand.Title) &&
            error.ErrorMessage == "Title is required.");
    }

    [Fact]
    public void Validate_ShouldFail_WhenUserIdIsEmpty()
    {
        var command = CreateValidCommand() with { UserId = Guid.Empty };

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(CreateCashflowCommand.UserId) &&
            error.ErrorMessage == "UserId is required.");
    }

    private static CreateCashflowCommand CreateValidCommand() =>
        new("Cashflow pessoal", Guid.NewGuid());
}
