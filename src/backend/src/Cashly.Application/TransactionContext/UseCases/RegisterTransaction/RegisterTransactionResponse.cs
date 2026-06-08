namespace Cashly.Application.TransactionContext.UseCases.RegisterTransaction;

public sealed record CreateTransactionResponse(Guid TransactionId, decimal Amount, string Type);