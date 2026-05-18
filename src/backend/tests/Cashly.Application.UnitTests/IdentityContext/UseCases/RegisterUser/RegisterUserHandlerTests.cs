using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Abstractions.Persistence;
using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.IdentityContext.Interfaces.Security;
using Cashly.Application.IdentityContext.UseCases.RegisterUser;
using Cashly.Application.IdentityContext.UseCases.RegisterUser.Errors;
using Cashly.Application.Shared.Results;
using Cashly.Domain.IdentityContext.ValueObjects;
using Cashly.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.IdentityContext.UseCases.RegisterUser;

public sealed class RegisterUserHandlerTests
{
    [Fact]
    public async Task RegisterUser_ShouldRegister_WhenUserIsValid()
    {
        // arrange 
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var command = new RegisterUserCommand ("João", "Silva", "joao.silva@email.com", "password1234");

        userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<Email>())).ReturnsAsync(false);
        
        passwordHasherMock.Setup(x => x.Hash("password1234")).Returns("hashedPassword1234");
        
        var mediator = CreateMediator(
            userRepositoryMock,
            passwordHasherMock,
            unitOfWorkMock);
       
        // act 
        var result = await mediator.SendAsync(command);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        userRepositoryMock.Verify(x => x.ExistByEmailAsync(It.IsAny<Email>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        passwordHasherMock.Verify(x => x.Hash("password1234"), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldNotRegister_WhenEmailAlreadyExists()
    {
        // arrange 
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var command = new RegisterUserCommand("João", "Silva", "joao.silva@email.com", "password1234");
        
        userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<Email>())).ReturnsAsync(true);
        
        var mediator = CreateMediator(
            userRepositoryMock,
            passwordHasherMock,
            unitOfWorkMock);
        
        // act 
        var result = await mediator.SendAsync(command);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(RegisterUserApplicationErrors.EmailAlreadyExists);
        
        passwordHasherMock.Verify(x => x.Hash(It.IsAny<string>()), Times.Never);
        
        userRepositoryMock.Verify(x => x.ExistByEmailAsync(It.IsAny<Email>()), Times.Once);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }

    private static IMediator CreateMediator(
        Mock<IUserRepository> userRepositoryMock,
        Mock<IPasswordHasher> passwordHasherMock,
        Mock<IUnitOfWork> unitOfWorkMock)
    {
        var services = new ServiceCollection();

        services.AddSingleton(userRepositoryMock.Object);
        services.AddSingleton(passwordHasherMock.Object);
        services.AddSingleton(unitOfWorkMock.Object);
        services.AddSingleton<ICommandHandler<RegisterUserCommand, Result<RegisterUserResponse>>, RegisterUserHandler>();
        services.AddSingleton<IMediator, Mediator>();

        return services.BuildServiceProvider().GetRequiredService<IMediator>();
    }
}
