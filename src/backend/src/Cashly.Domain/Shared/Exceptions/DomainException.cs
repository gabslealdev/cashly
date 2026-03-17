using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.Shared.Exceptions
{
    public class DomainException : Exception
    {
        public DomainError Error { get; }

        public string Code => Error.Code;

        public DomainException(DomainError error) : base(error.Message)
        {
            Error = error;
        }
    }
}
