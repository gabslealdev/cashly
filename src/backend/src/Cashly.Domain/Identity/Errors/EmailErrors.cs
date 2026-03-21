using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.Identity.Errors
{
    public static class EmailErrors
    {
        public readonly static DomainError EmailRequired = new DomainError("User.Email.EmailRequired", "Email is required.");
        public readonly static DomainError EmailFormatInvalid = new DomainError("User.Email.EmailFormatInvalid", "Email is invalid.");
        public readonly static DomainError EmailTooShort = new DomainError("User.Email.EmailTooShort", "Email is too short.");
        public readonly static DomainError EmailTooLong = new DomainError("User.Email.EmailTooLong", "Email is too long.");
    }
}
