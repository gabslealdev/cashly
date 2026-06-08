using Cashly.Domain.Shared.Errors;

namespace Cashly.Application.Shared.Results
{
    public sealed record ApplicationError(string Code, string Message)
    {
        public static readonly ApplicationError None = new(string.Empty, string.Empty);

        public static ApplicationError FromDomain(DomainError error)
            => new(error.Code, error.Message);
    }
}
