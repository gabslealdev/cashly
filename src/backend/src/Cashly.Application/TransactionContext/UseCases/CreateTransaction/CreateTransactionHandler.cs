using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Abstractions.Persistence;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;
using Cashly.Application.TransactionContext.Error;
using Cashly.Application.TransactionContext.Interfaces.Repository;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Entity;
using Cashly.Domain.TransactionContext.Enums;
using Cashly.Domain.TransactionContext.ValueObjects;

namespace Cashly.Application.TransactionContext.UseCases.CreateTransaction;

public sealed class CreateTransactionHandler : ICommandHandler<CreateTransactionCommand
    , Result<CreateTransactionResponse>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICashflowMemberReadRepository  _cashflowMemberReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionHandler(ITransactionRepository transactionRepository, 
        ICashflowMemberReadRepository cashflowMemberReadRepository, 
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _cashflowMemberReadRepository = cashflowMemberReadRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<CreateTransactionResponse>> HandleAsync(CreateTransactionCommand command)
    {
        var hasPermission = await _cashflowMemberReadRepository.CanCreateTransactionAsync(
            command.UserId, command.CashflowId);
        
        if (!hasPermission)
            return Result<CreateTransactionResponse>.Failure(CreateTransactionErrors.InvalidStatus);

        if (!Enum.TryParse<TransactionType>(command.Type, out var type))
            return Result<CreateTransactionResponse>.Failure(CreateTransactionErrors.InvalidType);

        if (!Enum.TryParse<TransactionStatus>(command.Status, out var status))
            return Result<CreateTransactionResponse>.Failure(CreateTransactionErrors.InvalidStatus);

        var transaction = Transaction.Create(
            cashflowId: command.CashflowId,
            title: Title.Create(command.Title),
            amount: Amount.Create(command.Amount),
            type: type,
            date: command.Date,
            status: status);
        
        await _transactionRepository.AddAsync(transaction);
        await _unitOfWork.CommitAsync();
        
        var response = new CreateTransactionResponse(transaction.Id, transaction.Amount.Value, transaction.Type.ToString());
        
        return Result<CreateTransactionResponse>.Success(response);
    }
}