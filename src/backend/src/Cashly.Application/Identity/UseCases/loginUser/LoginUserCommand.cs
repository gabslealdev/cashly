namespace Cashly.Application.Identity.UseCases.loginUser
{
    public sealed record LoginUserCommand
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
