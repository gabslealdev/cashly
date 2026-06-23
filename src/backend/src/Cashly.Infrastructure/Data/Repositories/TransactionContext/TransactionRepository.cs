using Cashly.Application.TransactionContext.Interfaces.Repository;
using Cashly.Domain.TransactionContext.Entity;
using Cashly.Infrastructure.Data.Context;

namespace Cashly.Infrastructure.Data.Repositories.TransactionContext;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly CashlyDbContext _context;

    public TransactionRepository(CashlyDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }
}