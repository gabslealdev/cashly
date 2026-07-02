using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;
using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;
using Cashly.Domain.CashflowContext.Entities;

namespace Cashly.Application.CashflowContext.Interfaces.Repository;

public interface ICashflowReadRepository
{
    Task<IReadOnlyList<UserCashflowReadModel>> GetUserCashflowsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
    
    Task<CashflowBoardHeaderReadModel?> GetCashflowBoardHeaderAsync(
        Guid cashflowId,
        Guid userId,
        CancellationToken cancellationToken = default);
    
    Task<Cashflow?> GetCashflowById(Guid cashflowId, CancellationToken cancellationToken = default);
}
