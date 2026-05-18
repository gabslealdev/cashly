using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.IdentityContext.UseCases.RegisterUser
{
    public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string Password)
        : ICommand<Result<RegisterUserResponse>>;
}
