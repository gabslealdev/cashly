using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Domain.CollaborationContext.Enums;
using Cashly.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashly.Infrastructure.Data.Repositories.CashflowContext;

public sealed class CashflowMemberReadRepository : ICashflowMemberReadRepository
{
    private readonly CashlyDbContext _context;

    public CashflowMemberReadRepository(CashlyDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> HasMemberAsync(
        Guid userId,
        Guid cashflowId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CashflowMembers
            .AsNoTracking()
            .AnyAsync(member => 
                member.UserId == userId &&
                member.CashflowId == cashflowId,
                cancellationToken);
    }
}
