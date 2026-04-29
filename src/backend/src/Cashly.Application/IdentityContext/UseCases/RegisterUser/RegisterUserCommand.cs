namespace Cashly.Application.Identity.UseCases.RegisterUser
{
    public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string Password);
}
