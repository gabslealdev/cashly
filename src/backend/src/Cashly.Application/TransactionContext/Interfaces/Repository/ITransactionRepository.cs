using Cashly.Domain.TransactionContext.Entity;

namespace Cashly.Application.TransactionContext.Interfaces.Repository;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
}