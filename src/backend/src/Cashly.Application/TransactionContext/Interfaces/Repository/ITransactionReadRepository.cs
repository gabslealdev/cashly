using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;

namespace Cashly.Application.TransactionContext.Interfaces.Repository;

public interface ITransactionReadRepository
{
    Task<IReadOnlyList<CashflowBoardTransactionReadModel>> GetBoardTransactionAsync(
        Guid cashflowId, 
        DateTimeOffset startDate,
        DateTimeOffset endDate);
}