using Cashly.Domain.Identity.Errors;
using Cashly.Domain.Shared.Exceptions;
using Cashly.Domain.Shared.ValueObjects;

namespace Cashly.Domain.Identity.ValueObjects
{
    public sealed record PasswordHash : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        private PasswordHash() { }

        private PasswordHash(string value)
        {
            Value = value;
        }

        public static PasswordHash Create(string value)
        {
            var normalizedValue = Normalized(value);
            Validate(normalizedValue);

            return new PasswordHash(normalizedValue);
        }

        private static string Normalized(string value)
            => value.Trim();

        private static void Validate(string value)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(value), UserErrors.PasswordRequired);
        }

    }
}
