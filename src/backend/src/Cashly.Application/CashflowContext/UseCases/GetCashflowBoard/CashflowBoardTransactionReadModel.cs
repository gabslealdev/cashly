namespace Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;

public record CashflowBoardTransactionReadModel(
    Guid TransactionId,
    string Title,
    decimal Amount,
    string Type,
    DateTimeOffset Date,
    string Status);