using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.IdentityContext.Interfaces.Security;
using Cashly.Application.IdentityContext.Models;
using Cashly.Application.IdentityContext.UseCases.LoginUser;
using Cashly.Application.Shared.Results;
using Cashly.Domain.IdentityContext.Entities;
using Cashly.Domain.IdentityContext.ValueObjects;
using Cashly.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.IdentityContext.UseCases.LoginUser;

public class LoginUserHandlerTests
{
    [Fact]
    public async Task LoginUser_ShouldLogin_WhenDataIsValid()
    {
        // arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        const string email = "joao.silva@email.com";
        const string password = "password123";
        var command = new LoginUserCommand(email, password);
        var user = User.Create(
            Name.Create("Joao", "Silva"),
            Email.Create(email),
            PasswordHash.Create("hashedPassword"));

        userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<Email>())).ReturnsAsync(user);

        passwordHasherMock.Setup(x => x.Verify(password, user.PasswordHash)).Returns(true);

        jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(user.Id, user.Email.Value))
            .Returns(new JwtTokenResult("token", DateTime.UtcNow.AddHours(1)));

        var mediator = CreateMediator(
            userRepositoryMock,
            passwordHasherMock,
            jwtTokenGeneratorMock);
        
        // act
        var result = await mediator.SendAsync(command);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        userRepositoryMock.Verify(x => x.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
        passwordHasherMock.Verify(x => x.Verify(password, user.PasswordHash), Times.Once);
        jwtTokenGeneratorMock.Verify(x => x.GenerateToken(user.Id, user.Email.Value), Times.Once);

    }
    
    [Fact]
    public async Task LoginUser_ShouldNotLogin_WhenUserDoesNotExist()
    {
        // arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        
        var command = new LoginUserCommand("joaosilva@email.com", "password123");
        
        userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<Email>())).ReturnsAsync((User?)null);
        
        var mediator = CreateMediator(
            userRepositoryMock,
            passwordHasherMock,
            jwtTokenGeneratorMock);
        
        // act
        var result = await mediator.SendAsync(command);
        
        // assert
        result.IsSuccess.ShouldBeFalse();
        
        passwordHasherMock.Verify(
            x => x.Verify(It.IsAny<string>(), It.IsAny<PasswordHash>()),
            Times.Never);

        jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);

    }

    private static IMediator CreateMediator(
        Mock<IUserRepository> userRepositoryMock,
        Mock<IPasswordHasher> passwordHasherMock,
        Mock<IJwtTokenGenerator> jwtTokenGeneratorMock)
    {
        var services = new ServiceCollection();

        services.AddSingleton(userRepositoryMock.Object);
        services.AddSingleton(passwordHasherMock.Object);
        services.AddSingleton(jwtTokenGeneratorMock.Object);
        services.AddSingleton<ICommandHandler<LoginUserCommand, Result<LoginUserResponse>>, LoginUserHandler>();
        services.AddSingleton<IMediator, Mediator>();

        return services.BuildServiceProvider().GetRequiredService<IMediator>();
    }
}
