using Cashly.Domain.CashflowContext.ValueObjects;

namespace Cashly.Application.CashflowContext.UseCases.CreateCashflow;

public sealed record CreateCashflowCommand(string Title, Guid UserId);