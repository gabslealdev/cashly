namespace Cashly.Api.Contracts.TransactionContext.RegisterTransaction;

public record CreateTransactionResponseDto(Guid TransactionId, decimal Value, string Type);