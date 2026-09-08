using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Abstractions.Persistence;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;
using Cashly.Application.TransactionContext.Error;
using Cashly.Application.TransactionContext.Interfaces.Repository;
using Cashly.Application.TransactionContext.UseCases.RegisterTransaction;
using Cashly.Domain.CashflowContext.Entities;
using Cashly.Domain.CashflowContext.Errors;
using Cashly.Domain.CashflowContext.Services;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Entity;
using Cashly.Domain.TransactionContext.Enums;
using Cashly.Domain.TransactionContext.ValueObjects;
using Cashly.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.TransactionContext.RegisterTransaction;

public class RegisterTransactionUnitTests
{
    [Fact]
    public async Task RegisterTransaction_ShouldRegister_WhenIsValid()
    {
        // arrange
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var cashflow = Cashflow.Create(
            Title.Create("Cashflow do João"),
            userId);
        
        var transactionDate = new DateTimeOffset(
            DateTimeOffset.Now.Year,
            DateTimeOffset.Now.Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);
            
        var command = new CreateTransactionCommand(
            userId,
            cashflow.Id,
            "Mensalidade Academia",
            500.00m,
            "Expense",
            transactionDate,
            "Completed"
            );
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowById(command.CashflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cashflow);

        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionRepositoryMock,
            unitOfWorkMock);
        
        // act
        var result = await mediator.SendAsync(command);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>())
            , Times.Once);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>())
            , Times.Once);
    }

    [Fact]
    public async Task RegisterTransaction_ShouldNotRegister_WhenCashflowIsNull()
    {
        // arrange 
        var userId = Guid.NewGuid();
        var cashflowId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        Cashflow? cashflow = null;
        
        var transactionDate = new DateTimeOffset(
            DateTimeOffset.Now.Year,
            DateTimeOffset.Now.Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);

        var command = new CreateTransactionCommand(
            userId,
            cashflowId,
            "Mensalidade Academia",
            500.00m,
            "Expense",
            transactionDate,
            "Completed"
        );
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowById(command.CashflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cashflow);

        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionRepositoryMock,
            unitOfWorkMock);
        
        // act 
        var result = await mediator.SendAsync(command);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(CreateTransactionErrors.CashflowNotFound.Code);
        result.Error.Message.ShouldBe(CreateTransactionErrors.CashflowNotFound.Message);
        
        transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>())
            ,Times.Never);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>())
        , Times.Never);
    }

    [Fact]
    public async Task RegisterTransaction_ShouldNotRegister_WhenMonthIsClosed() 
    {
        // arrange
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var calculator = new PeriodFinancialResultCalculator();
        var cashflow = Cashflow.Create(
            Title.Create("Cashflow do João"),
            userId);
        
        var referenceDate = new DateTimeOffset(
            DateTimeOffset.Now.Year,
            DateTimeOffset.Now.AddMonths(-1).Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);

        var expenseTransaction = Transaction.Create(
            Guid.NewGuid(),
            Title.Create("Financiamento Carro"),
            Amount.Create(800.00m),
            TransactionType.Expense,
            referenceDate,
            TransactionStatus.Completed
        );
        
        var incomeTransaction = Transaction.Create(           
            Guid.NewGuid(),
            Title.Create("Salario do mês"),
            Amount.Create(5800.00m),
            TransactionType.Income,
            referenceDate,
            TransactionStatus.Completed
        );

        var transaction = new[]
        {
            incomeTransaction,
            expenseTransaction
        };

        var period = Period.From(referenceDate);
        var periodFinancialResult = calculator.Calculate(period, transaction);
        var classifier = new FinancialHealthClassifier();
        var financialHealthStatus = classifier.Classify(periodFinancialResult);
        
        cashflow.CloseMonth(period, periodFinancialResult, financialHealthStatus, DateTimeOffset.UtcNow);
        
        var command = new CreateTransactionCommand(
            userId,
            cashflow.Id,
            "Mensalidade Academia",
            500.00m,
            "Expense",
            referenceDate.AddDays(7),
            "Completed"
        );
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowById(command.CashflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cashflow);

        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionRepositoryMock,
            unitOfWorkMock);
        
        // act
        var result =  await mediator.SendAsync(command);
        
        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(CashflowErrors.MonthIsClosed.Code);
        result.Error.Message.ShouldBe(CashflowErrors.MonthIsClosed.Message);
        
        transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>())
        ,Times.Never);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>())
            , Times.Never());
    }

    [Fact]
    public async Task RegisterTransaction_ShouldNotRegister_WhenTransactionTypeIsInvalid()
    {
        // arrange
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var cashflow = Cashflow.Create(
            Title.Create("Cashflow do João"),
            userId);
        
        var transactionDate = new DateTimeOffset(
            DateTimeOffset.Now.Year,
            DateTimeOffset.Now.Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);
            
        var command = new CreateTransactionCommand(
            userId,
            cashflow.Id,
            "Mensalidade Academia",
            500.00m,
            "Despesa",
            transactionDate,
            "Completed"
        );
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowById(command.CashflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cashflow);

        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionRepositoryMock,
            unitOfWorkMock);
        
        // act
        var result = await mediator.SendAsync(command);
        
        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(CreateTransactionErrors.InvalidType.Code);
        result.Error.Message.ShouldBe(CreateTransactionErrors.InvalidType.Message);
        
        transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>())
            , Times.Never);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>())
            , Times.Never());
    }
    
    [Fact]
    public async Task RegisterTransaction_ShouldNotRegister_WhenTransactionStatusIsInvalid()
    {
        // arrange
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var cashflow = Cashflow.Create(
            Title.Create("Cashflow do João"),
            userId);
        
        var transactionDate = new DateTimeOffset(
            DateTimeOffset.Now.Year,
            DateTimeOffset.Now.Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);
            
        var command = new CreateTransactionCommand(
            userId,
            cashflow.Id,
            "Mensalidade Academia",
            500.00m,
            "Expense",
            transactionDate,
            "Agendada"
        );
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowById(command.CashflowId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cashflow);

        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionRepositoryMock,
            unitOfWorkMock);
        
        // act
        var result = await mediator.SendAsync(command);
        
        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(CreateTransactionErrors.InvalidStatus.Code);
        result.Error.Message.ShouldBe(CreateTransactionErrors.InvalidStatus.Message);
        
        transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>())
            , Times.Never);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>())
            , Times.Never());
    }
    
    
    private static IMediator CreateMediator(
        Mock<ICashflowReadRepository> cashflowReadRepositoryMock,
        Mock<ITransactionRepository> transactionRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock)
    {
        var services = new ServiceCollection();
        
        services.AddSingleton(cashflowReadRepositoryMock.Object);
        services.AddSingleton(transactionRepositoryMock.Object);
        services.AddSingleton(unitOfWorkMock.Object);
        services.AddSingleton<ICommandHandler<CreateTransactionCommand, Result<CreateTransactionResponse>>,
        CreateTransactionHandler>();
        services.AddSingleton<IMediator, Mediator>();
        
        
        return services.BuildServiceProvider().GetRequiredService<IMediator>();
    }
}