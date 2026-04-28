using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.Errors;

public static class CreateCashflowErrors
{
    public static readonly Error UserNotFound = new Error("User.NotFound","User not found.");
}