namespace Cashly.Application.Identity.Models
{
    public record JwtTokenResult(string AccessToken, DateTime ExpiresAt);
}
