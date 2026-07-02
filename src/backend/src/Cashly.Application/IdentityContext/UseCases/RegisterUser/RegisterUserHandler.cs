using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Abstractions.Persistence;
using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.IdentityContext.Interfaces.Security;
using Cashly.Application.IdentityContext.UseCases.RegisterUser.Errors;
using Cashly.Application.Shared.Results;
using Cashly.Domain.IdentityContext.Entities;
using Cashly.Domain.IdentityContext.ValueObjects;

namespace Cashly.Application.IdentityContext.UseCases.RegisterUser
{
    public sealed class RegisterUserHandler : ICommandHandler<RegisterUserCommand, Result<RegisterUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHash,
            IUnitOfWork unitOfWork
            )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHash;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RegisterUserResponse>> HandleAsync(
            RegisterUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var name = Name.Create(command.FirstName, command.LastName);

            var email = Email.Create(command.Email);

            var emailExist = await _userRepository.ExistByEmailAsync(email, cancellationToken);

            if (emailExist)
                return Result<RegisterUserResponse>.Failure(RegisterUserApplicationErrors.EmailAlreadyExists);

            var hashedPassword = _passwordHasher.Hash(command.Password);
            var passwordHash = PasswordHash.Create(hashedPassword);

            var user = User.Create(name, email, passwordHash);
            var response = new RegisterUserResponse(user.Id);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<RegisterUserResponse>.Success(response);
        }

    }
}
                    
