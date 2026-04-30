namespace Cashly.Application.IdentityContext.UseCases.RegisterUser
{
    public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string Password);
}
