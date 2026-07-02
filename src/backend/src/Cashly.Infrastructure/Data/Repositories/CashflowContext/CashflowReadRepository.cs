using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;
using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;
using Cashly.Domain.CashflowContext.Entities;
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
    public async Task<IReadOnlyList<UserCashflowReadModel>> GetUserCashflowsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
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
            )).ToListAsync(cancellationToken);
        
        return cashflows;
    }

    public async Task<CashflowBoardHeaderReadModel?> GetCashflowBoardHeaderAsync(
        Guid cashflowId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cashflows
            .AsNoTracking()
            .Where(cashflow => cashflow.Id == cashflowId &&
                               cashflow.CashflowMembers.Any(member => member.UserId == userId))
            .Select(cashflow => new CashflowBoardHeaderReadModel(
                cashflow.Id,
                cashflow.Title.Value,
                cashflow.CashflowMembers
                    .Where(member => member.UserId == userId)
                    .Select(member => member.Role.ToString())
                    .First()
            )).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Cashflow?> GetCashflowById(
        Guid cashflowId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cashflows
            .Include(cashflow => cashflow.CashflowMembers)
            .Include(cashflow => cashflow.ClosedMonths)
            .FirstOrDefaultAsync(x => x.Id == cashflowId, cancellationToken);
    }
}
