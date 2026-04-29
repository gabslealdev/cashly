using Cashly.Application.Shared.Results;

namespace Cashly.Application.Identity.UseCases.loginUser.Errors
{
    public static class LoginUserApplicationErrors
    {
        public static readonly Error InvalidCredentials = new("Identity.LoginUser.InvalidCredentials", "Invalid email or password.");
    }
}
