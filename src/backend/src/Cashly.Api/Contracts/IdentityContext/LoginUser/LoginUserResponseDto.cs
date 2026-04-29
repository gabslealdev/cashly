namespace Cashly.Api.Contracts.IdentityContext.LoginUser
{
    public sealed record LoginUserResponseDto(string AccessToken, DateTime ExpiresAt);
}
