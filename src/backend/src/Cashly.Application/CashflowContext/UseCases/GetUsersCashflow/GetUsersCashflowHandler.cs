using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.UseCases.GetUsersCashflow;

public sealed class GetUsersCashflowHandler : IQueryHandler<GetUsersCashflowQuery, Result<GetUsersCashflowResponse>>
{
    private readonly ICashflowReadRepository _cashflowReadRepository;

    public GetUsersCashflowHandler(ICashflowReadRepository  cashflowReadRepository)
    {
        _cashflowReadRepository = cashflowReadRepository;
    }

    public async Task<Result<GetUsersCashflowResponse>> HandleAsync(
        GetUsersCashflowQuery query,
        CancellationToken cancellationToken = default)
    {
        var cashflows = await _cashflowReadRepository.GetUserCashflowsAsync(query.UserId, cancellationToken);
        
        var response = new GetUsersCashflowResponse(cashflows);
        
        return Result<GetUsersCashflowResponse>.Success(response);
    }
    
}
