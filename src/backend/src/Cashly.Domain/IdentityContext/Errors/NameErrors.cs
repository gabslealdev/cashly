using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.IdentityContext.Errors
{
    public static class NameErrors
    {
        public static readonly DomainError FirstNameRequired = new("User.Name.FirstNameRequired", "First name is required.");
        public static readonly DomainError FirstNameTooShort = new("User.Name.FirstNameTooShort", "First name is too short.");
        public static readonly DomainError FirstNameTooLong = new("User.Name.FirstNameTooLong", "First name is too long.");

        public static readonly DomainError LastNameRequired = new("User.Name.LastNameRequired", "Last name is required.");
        public static readonly DomainError LastNameTooShort = new("User.Name.LastNameTooShort", "Last name is too short.");
        public static readonly DomainError LastNameTooLong = new("User.Name.LastNameTooLong", "Last name is too long.");
    }
}
