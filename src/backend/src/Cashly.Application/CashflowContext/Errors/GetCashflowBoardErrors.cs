using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.Errors;

public static class GetCashflowBoardErrors
{
    public static readonly Error CashflowNotFound = new Error("Cashflow.NotFound", "Cashflow not found.");
    public static readonly Error HeaderNotFound = new Error("Header.NotFound", "Header not found.");
}