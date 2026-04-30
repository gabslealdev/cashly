using Cashly.Application.IdentityContext.Models;

namespace Cashly.Application.IdentityContext.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        JwtTokenResult GenerateToken(Guid userId, string email);
    }

}
