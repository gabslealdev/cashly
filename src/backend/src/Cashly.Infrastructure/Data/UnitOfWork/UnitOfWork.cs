using Cashly.Application.Shared.Abstractions;
using Cashly.Infrastructure.Data.Context;

namespace Cashly.Infrastructure.Data.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly CashlyDbContext _context;

        public UnitOfWork(CashlyDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        
    }
}
