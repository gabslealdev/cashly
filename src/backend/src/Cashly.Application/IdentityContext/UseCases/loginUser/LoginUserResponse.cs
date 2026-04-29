namespace Cashly.Application.IdentityContext.UseCases.loginUser
{
    public sealed record LoginUserResponse(string AccessToken, DateTime ExpiresAt);
}
