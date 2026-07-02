using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.Errors;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;
using Cashly.Application.TransactionContext.Interfaces.Repository;
using Cashly.Domain.CashflowContext.Services;
using Cashly.Domain.CashflowContext.ValueObjects;

namespace Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;

public class GetCashflowBoardHandler : IQueryHandler<GetCashflowBoardQuery,Result<GetCashflowBoardResponse>>
{
    private readonly ICashflowMemberReadRepository _cashflowMemberReadRepository;
    private readonly ICashflowReadRepository _cashflowReadRepository;
    private readonly ITransactionReadRepository _transactionReadRepository;

    public GetCashflowBoardHandler(
        ICashflowMemberReadRepository cashflowMemberReadRepository,
        ICashflowReadRepository cashflowReadRepository,
        ITransactionReadRepository transactionReadRepository)
    {
        _cashflowMemberReadRepository = cashflowMemberReadRepository;
        _cashflowReadRepository = cashflowReadRepository;
        _transactionReadRepository = transactionReadRepository;
    }
    
    public async Task<Result<GetCashflowBoardResponse>> HandleAsync(
        GetCashflowBoardQuery query,
        CancellationToken cancellationToken = default)
    {
        var isMember = await _cashflowMemberReadRepository.HasMemberAsync(
            query.UserId,
            query.CashflowId,
            cancellationToken);

        if (!isMember)
            return Result<GetCashflowBoardResponse>.Failure(GetCashflowBoardErrors.CashflowNotFound);
        
        var header = await _cashflowReadRepository.GetCashflowBoardHeaderAsync(
            query.CashflowId,
            query.UserId,
            cancellationToken);

        if (header is  null)
            return Result<GetCashflowBoardResponse>.Failure(GetCashflowBoardErrors.HeaderNotFound);
        
        var currentMonth = new DateTimeOffset(
            DateTimeOffset.UtcNow.Year,
            DateTimeOffset.UtcNow.Month,
            1, 0, 0, 0, TimeSpan.Zero);
        
        var startDate = currentMonth.AddMonths(-2);
        var endDate = startDate.AddMonths(4);
        
        var transactions = await _transactionReadRepository
            .GetBoardTransactionAsync(query.CashflowId, startDate, endDate, cancellationToken);
        
        
        var months = BuildMonthColumns(DateTime.UtcNow, transactions);

        var response = new GetCashflowBoardResponse(
            CashflowId: header.CashflowId,
            Title: header.Title,
            UserRole: header.UserRole,
            Months: months
        );
        
        return Result<GetCashflowBoardResponse>.Success(response);
    }
    
    private static IReadOnlyList<CashflowBoardMonthResponse> BuildMonthColumns(
        DateTime referenceDate, 
        IReadOnlyList<CashflowBoardTransactionReadModel> transactions)
    {
        var currentMonth = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        
        return Enumerable.Range(-2, 4)
            .Select(offset =>
            {
                var month = currentMonth.AddMonths(offset);

                var monthTransactions = transactions
                    .Where(transaction =>
                        transaction.Date.Year == month.Year &&
                        transaction.Date.Month == month.Month)
                    .ToList();
                
                var totalIncome = monthTransactions
                    .Where(transaction => transaction.Type == "Income" && transaction.Status == "Completed")
                    .Sum(transaction => transaction.Amount);
                
                var totalExpense = monthTransactions
                    .Where(transaction =>   transaction.Type == "Expense" && transaction.Status == "Completed")
                    .Sum(transaction => transaction.Amount);
                
                var periodFinancialResult = PeriodFinancialResult.Create(
                    Money.Create(totalIncome),
                    Money.Create(totalExpense));

                var balance = periodFinancialResult.PeriodResult.Value;
                
                var projected = monthTransactions
                    .Where(transaction => transaction.Status != "Cancelled")
                    .Sum(transaction => transaction.Type == "Income" ? transaction.Amount : -transaction.Amount);

                var financialHealth = new FinancialHealthClassifier();
                var healthStatus = financialHealth.Classify(periodFinancialResult);
                
                return new CashflowBoardMonthResponse(
                    Year: month.Year,
                    Month: month.Month,
                    Period: $"{month.Month:D2}/{month.Year:D4}",
                    Balance: balance,
                    Projected: projected,
                    IsClosed: false,
                    FinancialHealthStatus: healthStatus.ToString(),
                    Transactions: monthTransactions
                        .Select(transaction =>
                            new CashflowBoardTransactionResponse(
                                transaction.TransactionId,
                                transaction.Title,
                                transaction.Amount,
                                transaction.Type,
                                transaction.Date,
                                transaction.Status))
                        .ToList()
                );
            }).ToList();
    }
}
