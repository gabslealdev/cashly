using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;

namespace Cashly.Application.CashflowContext.Interfaces.Repository;

public interface ICashflowReadRepository
{
    Task<IReadOnlyList<UserCashflowReadModel>> GetUserCashflowsAsync(Guid userId);
}