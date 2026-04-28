using Cashly.Application.CashflowContext.Errors;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Identity.Interfaces.Repository;
using Cashly.Application.Shared.Abstractions;
using Cashly.Application.Shared.Results;
using Cashly.Domain.CashflowContext.Entities;
using Cashly.Domain.CashflowContext.ValueObjects;

namespace Cashly.Application.CashflowContext.UseCases.CreateCashflow;

public class CreateCashflowHandler
{
    private readonly ICashflowRepository _cashflowRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCashflowHandler(ICashflowRepository  cashflowRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _cashflowRepository = cashflowRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateCashflowResponse>> HandleAsync(CreateCashflowCommand command)
    {
        var userExists = await _userRepository.ExistByIdAsync(command.UserId);

        if (!userExists)
            return Result<CreateCashflowResponse>.Failure(CreateCashflowErrors.UserNotFound);
        
        var title = Title.Create(command.Title);
        var cashflow = Cashflow.Create(title, command.UserId);

        
        await _cashflowRepository.AddAsync(cashflow);
        await _unitOfWork.CommitAsync();
        
        return Result<CreateCashflowResponse>.Success(new CreateCashflowResponse(cashflow.Id, cashflow.Title.Value));
    }
}