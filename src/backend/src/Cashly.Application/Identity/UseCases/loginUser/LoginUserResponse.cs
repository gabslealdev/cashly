namespace Cashly.Application.Identity.UseCases.loginUser
{
    public sealed class LoginUserResponse
    {
        public string AccessToken { get; init; } = string.Empty;

        public DateTime ExpiresAt { get; init; }
    }
}
