using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.Errors;

public static class GetCashflowBoardErrors
{
    public static readonly ApplicationError CashflowNotFound = new ApplicationError("Cashflow.NotFound", "Cashflow not found.");
    public static readonly ApplicationError HeaderNotFound = new ApplicationError("Header.NotFound", "Header not found.");
}
