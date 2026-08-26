
using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Abstractions.Persistence;
using Cashly.Application.CashflowContext.Errors;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.CreateCashflow;
using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.Shared.Results;
using Cashly.Domain.CashflowContext.Entities;
using Cashly.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.CashflowContext.UseCases.CreateCashflow;

public sealed class CreateCashflowHandlerTests
{
    [Fact]
    public async Task CreateCashflow_ShouldCreate_WhenUserIsValid()
    {
        // arrange
        var userId = Guid.NewGuid();
        var cashflowRepositoryMock = new Mock<ICashflowRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var command = new CreateCashflowCommand("Cashflow do João",  userId);
        
        userRepositoryMock.Setup(x => x.ExistByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var mediator = CreateMediator(
            cashflowRepositoryMock,
            userRepositoryMock,
            unitOfWorkMock);
        
        // act
        var result = await mediator.SendAsync(command);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        userRepositoryMock.Verify(x => x.ExistByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
        
        cashflowRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Cashflow>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateCashflow_ShouldNotCreate_WhenUserIsNotValid()
    {
        // arrange
        var userId = Guid.Empty;
        var cashflowRepositoryMock = new Mock<ICashflowRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var command = new CreateCashflowCommand("Cashflow do João",  userId);

        userRepositoryMock.Setup(x => x.ExistByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var mediator = CreateMediator(
            cashflowRepositoryMock, 
            userRepositoryMock, 
            unitOfWorkMock);
        
        // act 
        var result = await mediator.SendAsync(command);
        
        // assert 
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CreateCashflowErrors.UserNotFound);
        
        userRepositoryMock.Verify(x => x.ExistByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    private static IMediator CreateMediator(
        Mock<ICashflowRepository> cashflowRepositoryMock,
        Mock<IUserRepository> userRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock)
    {
        var services = new ServiceCollection(); 
        
        services.AddSingleton(cashflowRepositoryMock.Object);
        services.AddSingleton(userRepositoryMock.Object);
        services.AddSingleton(unitOfWorkMock.Object);
        services.AddSingleton<ICommandHandler<CreateCashflowCommand, Result<CreateCashflowResponse>>, CreateCashflowHandler>();
        services.AddSingleton<IMediator, Mediator>();
        
        return services.BuildServiceProvider().GetRequiredService<IMediator>();
    }
}