namespace Cashly.Application.Identity.UseCases.loginUser
{
    public sealed record LoginUserResponse(string AccessToken, DateTime ExpiresAt);
}
