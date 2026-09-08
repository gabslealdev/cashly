using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.GetUsersCashflow;
using Cashly.Application.Shared.Results;
using Cashly.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.CashflowContext.UseCases.GetUsersCashflow;

public class GetUserCashflowUnitTests
{
    [Fact]
    public async Task GetUsersCashflow_ShouldGet_WhenIsValid()
    {
        var userId = Guid.NewGuid();
        var cashflowReadRepositoryMock = new Mock<ICashflowReadRepository>();

        var cashflow = new UsersCashflowReadModel(
            Guid.NewGuid(),
            "Cashflow do João",
            "Owner",
            1);

        var cashflows = new[]
        {
            cashflow
        };
        
        cashflowReadRepositoryMock.Setup(x => x.GetUserCashflowsAsync(
            userId, 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(cashflows);
        
        var query = new GetUsersCashflowQuery(userId);
        
        var mediator = CreateMediator(cashflowReadRepositoryMock);
        
        // act 
        var result = await mediator.SendAsync(query);
        
        // assert 
        result.IsSuccess.ShouldBeTrue(); 
    }
    
    
    private static IMediator CreateMediator(
        Mock<ICashflowReadRepository> cashflowReadRepositoryMock)
    {
        var services = new ServiceCollection();

        services.AddSingleton(cashflowReadRepositoryMock.Object);
        services.AddSingleton<IQueryHandler<GetUsersCashflowQuery, Result<GetUsersCashflowResponse>>
            , GetUsersCashflowHandler>();
        services.AddSingleton<IMediator, Mediator>();
        
        return services.BuildServiceProvider().GetRequiredService<IMediator>();
    }
}