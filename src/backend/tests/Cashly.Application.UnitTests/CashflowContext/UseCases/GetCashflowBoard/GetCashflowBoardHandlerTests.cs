using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.Errors;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;
using Cashly.Application.Shared.Results;
using Cashly.Application.TransactionContext.Interfaces.Repository;
using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.CashflowContext.UseCases.GetCashflowBoard;

public class GetCashflowBoardHandlerTests
{
    [Fact]
    public async Task GetCashflowBoard_ShouldGet_WhenHeaderIsValid()
    {
        // arrange
        var cashflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionReadRepositoryMock = new Mock<ITransactionReadRepository>();

        var header = new CashflowBoardHeaderReadModel(
            cashflowId,
            "Cashflow do João",
            "Owner");

        cashflowReadRepositoryMock.Setup(x => x.GetCashflowBoardHeaderAsync(cashflowId, userId,
            It.IsAny<CancellationToken>())).ReturnsAsync(header);

        transactionReadRepositoryMock.Setup(x => x.GetBoardTransactionAsync(
            cashflowId,
            It.IsAny<DateTimeOffset>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<CashflowBoardTransactionReadModel>());
        
        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionReadRepositoryMock);
        
        var query = new GetCashflowBoardQuery(userId, cashflowId);
        
        // act
        var result = await mediator.SendAsync(query);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        
        result.Value.CashflowId.ShouldBe(cashflowId);
        result.Value.Title.ShouldBe(header.Title);
        result.Value.UserRole.ShouldBe(header.UserRole);
        result.Value.Months.ShouldNotBeNull();
        result.Value.Months.Count.ShouldBe(4);
        
        cashflowReadRepositoryMock.Verify(
            repository => repository.GetCashflowBoardHeaderAsync(
                cashflowId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        transactionReadRepositoryMock.Verify(
            repository => repository.GetBoardTransactionAsync(
                cashflowId,
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCashflowBoard_ShouldThrow_WhenHeaderIsInvalid()
    {
        // arrange 
        var  cashflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        CashflowBoardHeaderReadModel? header = null;
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionReadRepositoryMock = new Mock<ITransactionReadRepository>();
        
        cashflowReadRepositoryMock.Setup(
            x => x.GetCashflowBoardHeaderAsync(cashflowId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(header);
        
        transactionReadRepositoryMock.Setup(x => x.GetBoardTransactionAsync(
                cashflowId,
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<CashflowBoardTransactionReadModel>());
        
        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionReadRepositoryMock);
        
        var query = new GetCashflowBoardQuery(userId, cashflowId);

        // act
        var result = await mediator.SendAsync(query);
        
        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(GetCashflowBoardErrors.HeaderNotFound);
        
        cashflowReadRepositoryMock.Verify(
            repository => repository.GetCashflowBoardHeaderAsync(
                cashflowId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        transactionReadRepositoryMock.Verify(
            repository => repository.GetBoardTransactionAsync(
                cashflowId,
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetCashflowBoard_ShouldGroupTransactionsByMonth()
    {
        // arrange
        var cashflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionReadRepositoryMock = new Mock<ITransactionReadRepository>();
        
         var header = new CashflowBoardHeaderReadModel(
             cashflowId,
             "Cashflow do João",
             "Owner");
         
         cashflowReadRepositoryMock.Setup(x => x.GetCashflowBoardHeaderAsync(
             cashflowId, 
             userId, 
             It.IsAny<CancellationToken>()
             )).ReturnsAsync(header);
         
         var currentDate = new  DateTimeOffset(
             DateTimeOffset.Now.Year,
             DateTimeOffset.Now.Month,
             1, 
             0,
             0,
             0,
             TimeSpan.Zero);

         var currentMonthExpenseTransaction = new CashflowBoardTransactionReadModel(
             Guid.NewGuid(),
             "Academia",
             250.00m,
             "Expense",
             currentDate.AddDays(3),
             "Completed"
         );

         var currentMonthIncomeTransaction = new CashflowBoardTransactionReadModel(
             Guid.NewGuid(),
             "Salário do mês",
             3500.00m,
             "Income",
             currentDate.AddDays(5),
             "Completed"
         );

         var previousMonthExpenseTransaction = new CashflowBoardTransactionReadModel(
             Guid.NewGuid(),
             "Parcela do Carro",
             760.00m,
             "Expense",
             currentDate.AddMonths(-1).AddDays(26),
             "Expense"
         );

         var previousMonthIncomeTransaction = new CashflowBoardTransactionReadModel(
             Guid.NewGuid(),
             "Salário do mês",
             3500.00m,
             "Income",
             currentDate.AddMonths(-1).AddDays(5),
             "Completed"
         );

         var transactions = new[]
         {
             previousMonthExpenseTransaction,
             previousMonthIncomeTransaction,
             currentMonthExpenseTransaction,
             currentMonthIncomeTransaction,
         };

         transactionReadRepositoryMock.Setup(x => x.GetBoardTransactionAsync(
             cashflowId,
             It.IsAny<DateTimeOffset>(),
             It.IsAny<DateTimeOffset>(),
             It.IsAny<CancellationToken>()
             )).ReturnsAsync(transactions);
         
         var query = new GetCashflowBoardQuery(userId, cashflowId);
         
         var mediator = CreateMediator(
             cashflowReadRepositoryMock, 
             transactionReadRepositoryMock);
         
         // act 
         var result = await mediator.SendAsync(query);
         
         // assert
         result.IsSuccess.ShouldBeTrue();

         var previousMonth = result.Value.Months.Single(month =>
             month.Year == currentDate.AddMonths(-1).Year && 
             month.Month == currentDate.AddMonths(-1).Month);

         var currentMonth = result.Value.Months.Single(month => 
             month.Year == currentDate.Year &&
             month.Month == currentDate.Month);
         
         previousMonth.Transactions.Count.ShouldBe(2);
         currentMonth.Transactions.Count.ShouldBe(2);
         
    }
    
    [Fact]
    public async Task GetCashflowBoard_ShouldCalculatePeriodFinancialResult()
    {
        // arrange 
        var cashflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionReadRepositoryMock = new Mock<ITransactionReadRepository>();

        var header = new CashflowBoardHeaderReadModel(
            cashflowId,
            "Cashflow do João",
            "Owner");
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowBoardHeaderAsync(
            cashflowId,  
            userId, 
            It.IsAny<CancellationToken>())).ReturnsAsync(header);

        var currentDate = new DateTimeOffset(
            DateTimeOffset.UtcNow.Year,
            DateTimeOffset.UtcNow.Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero
        );

        var expenseTransaction = new CashflowBoardTransactionReadModel(
            Guid.NewGuid(),
            "Mercado",
            1000.00m,
            "Expense",
            currentDate.AddDays(3),
            "Completed");

        var incomeTransaction = new CashflowBoardTransactionReadModel(
            Guid.NewGuid(),
            "Salário do Mês",
            2000.00m,
            "Income",
            currentDate.AddDays(5),
            "Completed");

        var transactions = new[]
        {
            expenseTransaction,
            incomeTransaction
        };
        
        transactionReadRepositoryMock.Setup(x => x.GetBoardTransactionAsync(
            cashflowId,
            It.IsAny<DateTimeOffset>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(transactions);
        
        var query = new GetCashflowBoardQuery(userId, cashflowId);
        
        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionReadRepositoryMock);
        
        // act
        var result = await mediator.SendAsync(query);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        var currentMonth = result.Value.Months.Single(month => 
            month.Year == currentDate.Year &&
            month.Month == currentDate.Month);

        currentMonth.Balance.ShouldBe(1000.00m);
    }
    
    [Fact]
    public async Task GetCashflowBoard_ShouldCalculateProjected()
    {
        // arrange 
        var cashflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionReadRepositoryMock = new Mock<ITransactionReadRepository>();

        var header = new CashflowBoardHeaderReadModel(
            cashflowId,
            "Cashflow do João",
            "Owner");
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowBoardHeaderAsync(
            cashflowId,  
            userId, 
            It.IsAny<CancellationToken>())).ReturnsAsync(header);

        var currentDate = new DateTimeOffset(
            DateTimeOffset.UtcNow.Year,
            DateTimeOffset.UtcNow.Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero
        );

        var expenseCompletedTransaction = new CashflowBoardTransactionReadModel(
            Guid.NewGuid(),
            "Mercado",
            1000.00m,
            "Expense",
            currentDate.AddDays(3),
            "Completed");

        var incomeCompletedTransaction = new CashflowBoardTransactionReadModel(
            Guid.NewGuid(),
            "Salário do Mês",
            2000.00m,
            "Income",
            currentDate.AddDays(5),
            "Completed");

        var incomeScheduledTransaction = new CashflowBoardTransactionReadModel(
            Guid.NewGuid(),
            "Estorno do Cartão de Crédito",
            1000.00m,
            "Income",
            currentDate.AddDays(7),
            "Scheduled");

        var transactions = new[]
        {
            expenseCompletedTransaction,
            incomeCompletedTransaction,
            incomeScheduledTransaction
        };
        
        transactionReadRepositoryMock.Setup(x => x.GetBoardTransactionAsync(
            cashflowId,
            It.IsAny<DateTimeOffset>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(transactions);
        
        var query = new GetCashflowBoardQuery(userId, cashflowId);
        
        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionReadRepositoryMock);
        
        // act
        var result = await mediator.SendAsync(query);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        var currentMonth = result.Value.Months.Single(month => 
            month.Year == currentDate.Year &&
            month.Month == currentDate.Month);

        currentMonth.Projected.ShouldBe(2000.00m);
    }
    
    [Fact]
    public async Task GetCashflowBoard_ShouldClassifyFinancialHealth()
    {
        // arrange 
        var cashflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();
        var transactionReadRepositoryMock = new Mock<ITransactionReadRepository>();

        var header = new CashflowBoardHeaderReadModel(
            cashflowId,
            "Cashflow do João",
            "Owner");
        
        cashflowReadRepositoryMock.Setup(x => x.GetCashflowBoardHeaderAsync(
            cashflowId,  
            userId, 
            It.IsAny<CancellationToken>())).ReturnsAsync(header);

        var currentDate = new DateTimeOffset(
            DateTimeOffset.UtcNow.Year,
            DateTimeOffset.UtcNow.Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero
        );

        var expenseTransaction = new CashflowBoardTransactionReadModel(
            Guid.NewGuid(),
            "Mercado",
            1000.00m,
            "Expense",
            currentDate.AddDays(3),
            "Completed");

        var incomeTransaction = new CashflowBoardTransactionReadModel(
            Guid.NewGuid(),
            "Salário do Mês",
            2000.00m,
            "Income",
            currentDate.AddDays(5),
            "Completed");

        var transactions = new[]
        {
            expenseTransaction,
            incomeTransaction,
        };
        
        transactionReadRepositoryMock.Setup(x => x.GetBoardTransactionAsync(
            cashflowId,
            It.IsAny<DateTimeOffset>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(transactions);
        
        var query = new GetCashflowBoardQuery(userId, cashflowId);
        
        var mediator = CreateMediator(
            cashflowReadRepositoryMock,
            transactionReadRepositoryMock);
        
        // act
        var result = await mediator.SendAsync(query);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        var currentMonth = result.Value.Months.Single(month => 
            month.Year == currentDate.Year &&
            month.Month == currentDate.Month);

        currentMonth.FinancialHealthStatus.ShouldBe(nameof(FinancialHealthStatus.Excellent));
        currentMonth.Period.ShouldBe(Period.Create(currentDate.Year, currentDate.Month).ToString());
    }
    
    
    private static IMediator CreateMediator(
        Mock<ICashflowReadRepository> cashflowReadRepositoryMock,
        Mock<ITransactionReadRepository> transactionReadRepositoryMock)
    {
        var services = new ServiceCollection(); 
        
        services.AddSingleton(cashflowReadRepositoryMock.Object);
        services.AddSingleton(transactionReadRepositoryMock.Object);
        services.AddSingleton<IQueryHandler<GetCashflowBoardQuery, Result<GetCashflowBoardResponse>>
            , GetCashflowBoardHandler>();
        services.AddSingleton<IMediator, Mediator>();
        
        return services.BuildServiceProvider().GetRequiredService<IMediator>();
        
    }
}