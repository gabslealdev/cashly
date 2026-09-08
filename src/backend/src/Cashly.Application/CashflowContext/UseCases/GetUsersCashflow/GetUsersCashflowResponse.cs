namespace Cashly.Application.CashflowContext.UseCases.GetUsersCashflow;

public sealed record GetUsersCashflowResponse(IReadOnlyList<UsersCashflowReadModel> Cashflows);
