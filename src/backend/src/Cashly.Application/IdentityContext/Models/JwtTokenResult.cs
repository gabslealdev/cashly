namespace Cashly.Application.IdentityContext.Models
{
    public record JwtTokenResult(string AccessToken, DateTime ExpiresAt);
}
