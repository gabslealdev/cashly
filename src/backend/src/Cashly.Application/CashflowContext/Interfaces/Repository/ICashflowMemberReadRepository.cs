namespace Cashly.Application.CashflowContext.Interfaces.Repository;

public interface ICashflowMemberReadRepository
{
    Task<bool> HasMemberAsync(Guid userId, Guid cashflowId);
}
