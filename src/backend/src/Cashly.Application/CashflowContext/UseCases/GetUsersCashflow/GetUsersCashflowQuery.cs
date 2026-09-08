using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.UseCases.GetUsersCashflow;

public sealed record GetUsersCashflowQuery(Guid UserId) : IQuery<Result<GetUsersCashflowResponse>>;
