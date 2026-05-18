using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.IdentityContext.UseCases.LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password)
        : ICommand<Result<LoginUserResponse>>;
}
