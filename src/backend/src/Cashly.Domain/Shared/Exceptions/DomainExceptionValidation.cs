using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.Shared.Exceptions
{
    public sealed class DomainExceptionValidation : DomainException
    {
        public DomainExceptionValidation(DomainError error) : base(error)
        {
        }

        public static void When(bool hasError, DomainError error)
        {
            if (hasError)
            {
                throw new DomainExceptionValidation(error);
            }
        }
    }
}
