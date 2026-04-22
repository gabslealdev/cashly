using Cashly.Application.Identity.Interfaces.Repository;
using Cashly.Application.Identity.Interfaces.Security;
using Cashly.Application.Identity.Models;
using Cashly.Application.Identity.UseCases.loginUser;
using Cashly.Domain.Identity.Entities;
using Cashly.Domain.Identity.ValueObjects;
using Moq;
using Shouldly;

namespace Cashly.Application.UnitTests.Identity.UseCases.LoginUser;

public class LoginUserHandlerTests
{
    [Fact]
    public async Task LoginUser_ShouldLogin_WhenDataIsValid()
    {
        // arrange
        var userRespositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        const string email = "joao.silva@email.com";
        const string password = "password123";
        var command = new LoginUserCommand(email, password);
        var user = User.Create(
            Name.Create("Joao", "Silva"),
            Email.Create(email),
            PasswordHash.Create("hashedPassword"));

        userRespositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<Email>())).ReturnsAsync(user);

        passwordHasherMock.Setup(x => x.Verify(password, user.PasswordHash)).Returns(true);

        jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(user.Id, user.Email.Value))
            .Returns(new JwtTokenResult("token", DateTime.UtcNow.AddHours(1)));

        var handler = new LoginUserHandler(userRespositoryMock.Object, 
            passwordHasherMock.Object, jwtTokenGeneratorMock.Object);
        
        // act
        var result = await handler.Handle(command);
        
        // assert
        result.IsSuccess.ShouldBeTrue();
        
        userRespositoryMock.Verify(x => x.GetByEmailAsync(It.IsAny<Email>()), Times.Once);
        passwordHasherMock.Verify(x => x.Verify(password, user.PasswordHash), Times.Once);
        jwtTokenGeneratorMock.Verify(x => x.GenerateToken(user.Id, user.Email.Value), Times.Once);

    }
    
    [Fact]
    public async Task LoginUser_ShouldNotLogin_WhenUserDoesNotExist()
    {
        // arrange
        var userRespositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        
        var command = new LoginUserCommand("joaosilva@email.com", "password123");
        
        userRespositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<Email>())).ReturnsAsync((User?)null);
        
        var handler = new LoginUserHandler(userRespositoryMock.Object, 
            passwordHasherMock.Object, jwtTokenGeneratorMock.Object);
        
        // act
        var result = await handler.Handle(command);
        
        // assert
        result.IsSuccess.ShouldBeFalse();
        
        passwordHasherMock.Verify(
            x => x.Verify(It.IsAny<string>(), It.IsAny<PasswordHash>()),
            Times.Never);

        jwtTokenGeneratorMock.Verify(
            x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);

    }
}