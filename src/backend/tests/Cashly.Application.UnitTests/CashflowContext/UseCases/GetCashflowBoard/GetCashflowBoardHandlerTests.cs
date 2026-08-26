using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;
using Cashly.Application.Shared.Results;
using Cashly.Application.TransactionContext.Interfaces.Repository;
using Cashly.Infrastructure.Messaging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.CashflowContext.UseCases.GetCashflowBoard;

public class GetCashflowBoardHandlerTests
{
    [Fact]
    public async Task GetCashflowBoard_Should_WhenIsValid()
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