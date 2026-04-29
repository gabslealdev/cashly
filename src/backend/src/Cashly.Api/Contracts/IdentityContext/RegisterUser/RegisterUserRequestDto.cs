namespace Cashly.Api.Contracts.IdentityContext.RegisterUser
{
    public sealed record RegisterUserRequestDto(string FirstName, string LastName, string Email, string Password);
}
