using Cashly.Application.Identity.Models;

namespace Cashly.Application.Identity.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        JwtTokenResult GenerateToken(Guid userId, string email);
    }

}
