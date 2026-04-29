namespace Cashly.Api.Contracts.Identity.LoginUser
{
    public sealed record LoginUserResponseDto(string AccessToken, DateTime ExpiresAt);
}
