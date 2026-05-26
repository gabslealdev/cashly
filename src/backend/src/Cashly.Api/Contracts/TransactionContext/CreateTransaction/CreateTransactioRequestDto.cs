namespace Cashly.Api.Contracts.TransactionContext.CreateTransaction;

public sealed record CreateTransactionRequestDto(
    string Title,
    decimal Amount,
    string Type,
    DateTimeOffset Date,
    string Status
    );