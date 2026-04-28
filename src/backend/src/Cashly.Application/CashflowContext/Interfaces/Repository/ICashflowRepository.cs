using Cashly.Domain.CashflowContext.Entities;

namespace Cashly.Application.CashflowContext.Interfaces.Repository;

public interface ICashflowRepository
{
    Task AddAsync(Cashflow cashflow);
}