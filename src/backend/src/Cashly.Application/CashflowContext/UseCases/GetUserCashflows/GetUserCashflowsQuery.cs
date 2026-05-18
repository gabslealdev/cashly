using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.UseCases.GetUserCashflows;

public sealed record GetUserCashflowsQuery(Guid UserId) : IQuery<Result<GetUserCashflowsResponse>>;
