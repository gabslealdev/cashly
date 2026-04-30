using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;
using Cashly.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashly.Infrastructure.Data.Repositories.CashflowContext;

public class CashflowReadRepository : ICashflowReadRepository
{
    private readonly CashlyDbContext _context;

    public CashflowReadRepository(CashlyDbContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyList<UserCashflowReadModel>> GetUserCashflowsAsync(Guid userId)
    {
        var cashflows = await _context.Cashflows
            .AsNoTracking()
            .Where(cashflow => cashflow.CashflowMembers.Any(cashflowMember => cashflowMember.UserId == userId))
            .Select(cashflow => new UserCashflowReadModel(
                cashflow.Id,
                cashflow.Title.Value,
                cashflow.CashflowMembers
                    .Where(member => member.UserId == userId)
                    .Select(member => member.Role.ToString())
                    .First(),
                cashflow.CashflowMembers.Count
            )).ToListAsync();
        
        return cashflows;
    }
}