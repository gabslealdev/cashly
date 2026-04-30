using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.IdentityContext.Errors
{
    public static class EmailErrors
    {
        public static readonly DomainError EmailRequired = new DomainError("User.Email.EmailRequired", "Email is required.");
        public static readonly DomainError EmailFormatInvalid = new DomainError("User.Email.EmailFormatInvalid", "Email is invalid.");
        public static readonly DomainError EmailTooShort = new DomainError("User.Email.EmailTooShort", "Email is too short.");
        public static readonly DomainError EmailTooLong = new DomainError("User.Email.EmailTooLong", "Email is too long.");
    }
}
