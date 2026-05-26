namespace Cashly.Application.TransactionContext.UseCases.CreateTransaction;

public sealed record CreateTransactionResponse(Guid TransactionId, decimal Amount, string Type);