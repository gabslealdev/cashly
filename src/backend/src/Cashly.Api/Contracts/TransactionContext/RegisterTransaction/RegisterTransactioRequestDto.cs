namespace Cashly.Api.Contracts.TransactionContext.RegisterTransaction;

public sealed record CreateTransactionRequestDto(
    string Title,
    decimal Amount,
    string Type,
    DateTimeOffset Date,
    string Status
    );