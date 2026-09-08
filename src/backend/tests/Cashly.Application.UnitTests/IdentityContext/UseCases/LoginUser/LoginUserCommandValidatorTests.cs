using Cashly.Application.IdentityContext.UseCases.LoginUser;
using Shouldly;

namespace Cashly.Application.UnitTests.IdentityContext.UseCases.LoginUser;

public sealed class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldSucceed_WhenCommandIsValid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Email", "Email is required.")]
    [InlineData("Password", "Password is required.")]
    public void Validate_ShouldFail_WhenRequiredFieldIsEmpty(
        string propertyName,
        string expectedMessage)
    {
        var validCommand = CreateValidCommand();
        var command = propertyName switch
        {
            nameof(LoginUserCommand.Email) => validCommand with { Email = string.Empty },
            nameof(LoginUserCommand.Password) => validCommand with { Password = string.Empty },
            _ => throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null)
        };

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error =>
            error.PropertyName == propertyName &&
            error.ErrorMessage == expectedMessage);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("joao@")]
    public void Validate_ShouldFail_WhenEmailFormatIsInvalid(string email)
    {
        var command = CreateValidCommand() with { Email = email };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(LoginUserCommand.Email) &&
            error.ErrorMessage == "Invalid email.");
    }

    private static LoginUserCommand CreateValidCommand() =>
        new("joao.silva@email.com", "password123");
}
