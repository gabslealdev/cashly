using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.Errors;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;

namespace Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;

public class GetCashflowBoardHandler : IQueryHandler<GetCashflowBoardQuery,Result<GetCashflowBoardResponse>>
{
    private readonly ICashflowMemberReadRepository _cashflowMemberReadRepository;
    private readonly ICashflowReadRepository _cashflowReadRepository;

    public GetCashflowBoardHandler(ICashflowMemberReadRepository cashflowMemberReadRepository,
        ICashflowReadRepository cashflowReadRepository)
    {
        _cashflowMemberReadRepository = cashflowMemberReadRepository;
        _cashflowReadRepository = cashflowReadRepository;
    }
    
    public async Task<Result<GetCashflowBoardResponse>> HandleAsync(GetCashflowBoardQuery query)
    {
        var isMember = await _cashflowMemberReadRepository.HasMemberAsync(query.UserId, query.CashflowId);

        if (!isMember)
            return Result<GetCashflowBoardResponse>.Failure(GetCashflowBoardErrors.CashflowNotFound);
        
        var header = await _cashflowReadRepository.GetCashflowBoardHeaderAsync(query.CashflowId, query.UserId);

        if (header is  null)
            return Result<GetCashflowBoardResponse>.Failure(GetCashflowBoardErrors.HeaderNotFound);

        var months = BuildMonthColumns(DateTime.UtcNow);

        var response = new GetCashflowBoardResponse(
            CashflowId: header.CashflowId,
            Title: header.Title,
            UserRole: header.UserRole,
            Months: months
        );
        
        return Result<GetCashflowBoardResponse>.Success(response);
    }
    
    private static IReadOnlyList<CashflowBoardMonthResponse> BuildMonthColumns(DateTime referenceDate)
    {
        var currentMonth = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        
        return Enumerable.Range(-2, 5)
            .Select(offset =>
            {
                var month = currentMonth.AddMonths(offset);

                return new CashflowBoardMonthResponse(
                    Year: month.Year,
                    Month: month.Month,
                    Period: $"{month.Month:D2}/{month.Year:D4}",
                    Balance: 0,
                    IsClosed: false,
                    Transactions: []
                );
            }).ToList();
    }
}