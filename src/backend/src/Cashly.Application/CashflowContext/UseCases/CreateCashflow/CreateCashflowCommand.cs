using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;
using Cashly.Domain.CashflowContext.ValueObjects;

namespace Cashly.Application.CashflowContext.UseCases.CreateCashflow;

public sealed record CreateCashflowCommand(string Title, Guid UserId) : ICommand<Result<CreateCashflowResponse>>;