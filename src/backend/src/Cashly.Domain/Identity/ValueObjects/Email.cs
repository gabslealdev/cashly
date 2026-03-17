using Cashly.Domain.Identity.Errors;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.Shared.ValueObjects;
using System.Text.RegularExpressions;

namespace Cashly.Domain.Identity.ValueObjects
{
    public sealed record Email : ValueObject
    {
        private const int MinLength = 8;
        private const int MaxLength = 255;
        public string Value { get; private set; } = string.Empty;

        private Email() { }

        private Email(string value )
        {
           Value = value;
        }

        public static Email Create(string value)
        {
            var normalizedValue = Normalize(value);
            Validate(normalizedValue);

            return new Email(normalizedValue);
        }

        private static string Normalize(string value)
            => value.Trim().ToLowerInvariant();

        private static bool IsValid(string value)
            => Regex.IsMatch(value, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");

        private static void Validate(string value)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(value), EmailErrors.EmailRequired);
            DomainExceptionValidation.When(value.Length <=  MinLength, EmailErrors.EmailTooShort);
            DomainExceptionValidation.When(value.Length >=  MaxLength, EmailErrors.EmailTooLong);
            DomainExceptionValidation.When(!IsValid(value), EmailErrors.EmailFormatInvalid);
        }

    }
}
