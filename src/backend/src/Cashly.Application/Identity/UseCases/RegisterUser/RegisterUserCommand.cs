namespace Cashly.Application.Identity.UseCases.RegisterUser
{
    public sealed record RegisterUserCommand
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
