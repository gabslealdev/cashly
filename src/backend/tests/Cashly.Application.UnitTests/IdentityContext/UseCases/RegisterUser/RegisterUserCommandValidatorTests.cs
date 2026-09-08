using Cashly.Application.IdentityContext.UseCases.RegisterUser;
using Shouldly;

namespace Cashly.Application.UnitTests.IdentityContext.UseCases.RegisterUser;

public sealed class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldSucceed_WhenCommandIsValid()
    {
        var command = CreateValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("FirstName", "First name is required.")]
    [InlineData("LastName", "Last name is required.")]
    [InlineData("Email", "Email is required.")]
    [InlineData("Password", "Password is required.")]
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

    [Theory]
    [InlineData("J", "First name must be at least 2 characters long.")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        "First name must be at most 80 characters long.")]
    public void Validate_ShouldFail_WhenFirstNameLengthIsInvalid(
        string firstName,
        string expectedMessage)
    {
        var command = CreateValidCommand() with { FirstName = firstName };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(RegisterUserCommand.FirstName) &&
            error.ErrorMessage == expectedMessage);
    }

    [Theory]
    [InlineData("S", "Last name must be at least 2 characters long.")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        "Last name must be at most 79 characters long.")]
    public void Validate_ShouldFail_WhenLastNameLengthIsInvalid(
        string lastName,
        string expectedMessage)
    {
        var command = CreateValidCommand() with { LastName = lastName };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(RegisterUserCommand.LastName) &&
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
            error.PropertyName == nameof(RegisterUserCommand.Email) &&
            error.ErrorMessage == "Invalid email.");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("1234567")]
    public void Validate_ShouldFail_WhenPasswordIsTooShort(string password)
    {
        var command = CreateValidCommand() with { Password = password };

        var result = _validator.Validate(command);

        result.Errors.ShouldContain(error =>
            error.PropertyName == nameof(RegisterUserCommand.Password) &&
            error.ErrorMessage == "Password must be at least 8 characters long.");
    }

    private static RegisterUserCommand CreateValidCommand() =>
        new("Joao", "Silva", "joao.silva@email.com", "password123");

    private static RegisterUserCommand CreateCommandWithEmptyProperty(string propertyName)
    {
        var command = CreateValidCommand();

        return propertyName switch
        {
            nameof(RegisterUserCommand.FirstName) => command with { FirstName = string.Empty },
            nameof(RegisterUserCommand.LastName) => command with { LastName = string.Empty },
            nameof(RegisterUserCommand.Email) => command with { Email = string.Empty },
            nameof(RegisterUserCommand.Password) => command with { Password = string.Empty },
            _ => throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null)
        };
    }
}
