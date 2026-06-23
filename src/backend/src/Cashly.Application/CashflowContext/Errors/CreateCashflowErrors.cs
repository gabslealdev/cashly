using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.Errors;

public static class CreateCashflowErrors
{
    public static readonly ApplicationError UserNotFound = new ApplicationError("User.NotFound", "User not found.");
}
