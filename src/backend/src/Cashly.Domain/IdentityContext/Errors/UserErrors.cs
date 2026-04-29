using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.Identity.Errors
{
    public static class UserErrors
    {
        public static DomainError PasswordRequired = new("User.PasswordHash.PasswordRequired", "Password hash is required");

    }
}
