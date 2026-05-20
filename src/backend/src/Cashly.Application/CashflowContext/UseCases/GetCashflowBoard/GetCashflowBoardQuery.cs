using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;

public sealed record GetCashflowBoardQuery(Guid UserId, Guid CashflowId) : IQuery<Result<GetCashflowBoardResponse>>;