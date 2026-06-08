namespace Cashly.Api.Contracts.TransactionContext.RegisterTransaction;

public record RegisterTransactionResponseDto(Guid TransactionId, decimal Value, string Type);