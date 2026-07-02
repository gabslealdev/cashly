using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;
using Cashly.Application.TransactionContext.Interfaces.Repository;
using Cashly.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashly.Infrastructure.Data.Repositories.TransactionContext;

public class TransactionReadRepository : ITransactionReadRepository
{
    private readonly CashlyDbContext _context;

    public TransactionReadRepository(CashlyDbContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyList<CashflowBoardTransactionReadModel>> 
        GetBoardTransactionAsync(
            Guid cashflowId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.CashflowId == cashflowId &&
                transaction.Date >= startDate &&
                transaction.Date < endDate)
            .OrderBy(transaction => transaction.Date)
            .Select(transaction => new CashflowBoardTransactionReadModel(
                transaction.Id,
                transaction.Title.Value,
                transaction.Amount.Value,
                transaction.Type.ToString(),
                transaction.Date,
                transaction.Status.ToString()))
            .ToListAsync(cancellationToken);
    }
}
