namespace Cashly.Application.IdentityContext.UseCases.LoginUser
{
    public sealed record LoginUserResponse(string AccessToken, DateTime ExpiresAt);
}
