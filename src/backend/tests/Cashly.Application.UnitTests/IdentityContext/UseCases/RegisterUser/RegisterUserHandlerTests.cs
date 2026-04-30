using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.IdentityContext.Interfaces.Security;
using Cashly.Application.IdentityContext.UseCases.RegisterUser;
using Cashly.Application.IdentityContext.UseCases.RegisterUser.Errors;
using Cashly.Application.Shared.Abstractions;
using Cashly.Domain.IdentityContext.ValueObjects;
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
        
        var handler = new RegisterUserHandler(userRepositoryMock.Object, 
            passwordHasherMock.Object, unitOfWorkMock.Object);
       
        // act 
        var result = await handler.HandleAsync(command);
        
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
        
        var handler = new RegisterUserHandler(userRepositoryMock.Object, passwordHasherMock.Object, unitOfWorkMock.Object);
        
        // act 
        var result = await handler.HandleAsync(command);

        // assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(RegisterUserApplicationErrors.EmailAlreadyExists);
        
        passwordHasherMock.Verify(x => x.Hash("password1234"), Times.Once);
        
        userRepositoryMock.Verify(x => x.ExistByEmailAsync(It.IsAny<Email>()), Times.Never);
        
        unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }
}