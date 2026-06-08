using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.TransactionContext.UseCases.RegisterTransaction;

public sealed record CreateTransactionCommand(
    Guid UserId,
    Guid CashflowId,
    string Title,
    decimal Amount,
    string Type,
    DateTimeOffset Date,
    string Status) : ICommand<Result<CreateTransactionResponse>>;