namespace Cashly.Application.CashflowContext.UseCases.GetUserCashflows;

public sealed record GetUserCashflowsResponse(IReadOnlyList<UserCashflowReadModel> Cashflows);
