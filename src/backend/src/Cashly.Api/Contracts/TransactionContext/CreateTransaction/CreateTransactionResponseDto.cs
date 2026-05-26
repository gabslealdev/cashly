namespace Cashly.Api.Contracts.TransactionContext.CreateTransaction;

public record CreateTransactionResponseDto(Guid TransactionId, decimal Value, string Type);