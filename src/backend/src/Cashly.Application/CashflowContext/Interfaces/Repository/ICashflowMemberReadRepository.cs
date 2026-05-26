namespace Cashly.Application.CashflowContext.Interfaces.Repository;

public interface ICashflowMemberReadRepository
{
    Task<bool> HasMemberAsync(Guid userId, Guid cashflowId);
    
    Task<bool> CanCreateTransactionAsync(Guid userId, Guid cashflowId);
}
