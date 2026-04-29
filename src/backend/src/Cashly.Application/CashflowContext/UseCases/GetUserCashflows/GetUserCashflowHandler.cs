using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.UseCases.GetUserCashflows;

public sealed class GetUserCashflowHandler
{
    private readonly ICashflowReadRepository _cashflowReadRepository;

    public GetUserCashflowHandler(ICashflowReadRepository  cashflowReadRepository)
    {
        _cashflowReadRepository = cashflowReadRepository;
    }

    public async Task<Result<GetUserCashflowsResponse>> HandleAsync(GetUserCashflowsQuery query)
    {
        var cashflows = await _cashflowReadRepository.GetUserCashflowsAsync(query.UserId);
        
        var response = new GetUserCashflowsResponse(cashflows);
        
        return Result<GetUserCashflowsResponse>.Success(response);
    }
    
}