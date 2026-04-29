namespace Cashly.Api.Contracts.Identity.RegisterUser
{
    public sealed record RegisterUserRequestDto(string FirstName, string LastName, string Email, string Password);
}
