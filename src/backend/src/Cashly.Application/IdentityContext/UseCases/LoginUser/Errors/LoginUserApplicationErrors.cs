using Cashly.Application.Shared.Results;

namespace Cashly.Application.IdentityContext.UseCases.LoginUser.Errors
{
    public static class LoginUserApplicationErrors
    {
        public static readonly ApplicationError InvalidCredentials = new("Identity.LoginUser.InvalidCredentials", "Invalid email or password.");
    }
}
