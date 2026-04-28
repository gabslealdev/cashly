using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Domain.CashflowContext.Entities;
using Cashly.Infrastructure.Data.Context;

namespace Cashly.Infrastructure.Data.Repositories.CashflowContext;

public class CashflowRepository : ICashflowRepository
{
    private readonly CashlyDbContext _context;
    
    public CashflowRepository(CashlyDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Cashflow cashflow)
    {
        await _context.Cashflows.AddAsync(cashflow);
    }
}