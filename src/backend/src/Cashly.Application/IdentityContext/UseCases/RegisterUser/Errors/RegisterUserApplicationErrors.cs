using Cashly.Application.Shared.Results;

namespace Cashly.Application.IdentityContext.UseCases.RegisterUser.Errors
{
    public static class RegisterUserApplicationErrors
    {
        public static readonly Error EmailAlreadyExists = new("Identity.RegisterUser.EmailAlreadyExist", "The email is already in use.");
    }
}
