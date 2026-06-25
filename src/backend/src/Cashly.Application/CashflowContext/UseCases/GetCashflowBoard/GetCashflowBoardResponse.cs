namespace Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;

public sealed record GetCashflowBoardResponse(
    Guid CashflowId, 
    string Title, 
    string UserRole, 
    IReadOnlyList<CashflowBoardMonthResponse> Months);

public sealed record CashflowBoardMonthResponse(
    int Year,
    int Month,
    string Period,
    decimal Balance,
    decimal Projected,
    bool IsClosed,
    string FinancialHealthStatus,
    IReadOnlyList<CashflowBoardTransactionResponse> Transactions);

public sealed record CashflowBoardTransactionResponse(
    Guid TransactionId,
    string Title,
    decimal Amount,
    string Type,
    DateTimeOffset Date,
    string Status
);
    
    