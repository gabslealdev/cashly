using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.UseCases.GetUserCashflows;

public sealed class GetUserCashflowHandler : IQueryHandler<GetUserCashflowsQuery, Result<GetUserCashflowsResponse>>
{
    private readonly ICashflowReadRepository _cashflowReadRepository;

    public GetUserCashflowHandler(ICashflowReadRepository  cashflowReadRepository)
    {
        _cashflowReadRepository = cashflowReadRepository;
    }

    public async Task<Result<GetUserCashflowsResponse>> HandleAsync(
        GetUserCashflowsQuery query,
        CancellationToken cancellationToken = default)
    {
        var cashflows = await _cashflowReadRepository.GetUserCashflowsAsync(query.UserId, cancellationToken);
        
        var response = new GetUserCashflowsResponse(cashflows);
        
        return Result<GetUserCashflowsResponse>.Success(response);
    }
    
}
