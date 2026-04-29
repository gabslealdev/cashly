using Cashly.Domain.IdentityContext.Errors;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.Shared.ValueObjects;

namespace Cashly.Domain.IdentityContext.ValueObjects
{
    public sealed record Name : ValueObject
    {
        private const int MinLength = 2;
        private const int MaxLength = 80;

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set;} = string.Empty;

        private Name(){}

        private Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static Name Create(string firstName, string lastName)
        {
            var normalizedFirstName = Normalize(firstName);
            var normalizedLastName = Normalize(lastName);
            Validate(normalizedFirstName, normalizedLastName);

            return new Name(normalizedFirstName, normalizedLastName);
        }

        public override string ToString() 
            => $"{FirstName} {LastName}";

        private static string Normalize(string value)
            => value.Trim();

        private static void Validate(string firstName, string lastName)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(firstName), NameErrors.FirstNameRequired);
            DomainExceptionValidation.When(firstName.Length <= MinLength, NameErrors.FirstNameTooShort);
            DomainExceptionValidation.When(firstName.Length >= MaxLength, NameErrors.FirstNameTooLong);

            DomainExceptionValidation.When(string.IsNullOrEmpty(lastName), NameErrors.LastNameRequired);
            DomainExceptionValidation.When(lastName.Length <= MinLength, NameErrors.LastNameTooShort);
            DomainExceptionValidation.When(lastName.Length >= MaxLength, NameErrors.LastNameTooLong);
        }

    }
}
