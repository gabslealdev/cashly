using Cashly.Application.Identity.Interfaces.Repository;
using Cashly.Application.Identity.Interfaces.Security;
using Cashly.Application.Identity.UseCases.RegisterUser.Errors;
using Cashly.Application.Shared.Abstractions;
using Cashly.Application.Shared.Results;
using Cashly.Domain.Identity.Entities;
using Cashly.Domain.Identity.ValueObjects;

namespace Cashly.Application.Identity.UseCases.RegisterUser
{
    public sealed class RegisterUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHash, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHash;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RegisterUserResponse>> HandleAsync(RegisterUserCommand command)
        {
            var name = Name.Create(command.FirstName, command.LastName);

            var email = Email.Create(command.Email);

            var emailExist = await _userRepository.ExistByEmailAsync(email);

            if (emailExist)
                return Result<RegisterUserResponse>.Failure(RegisterUserApplicationErrors.EmailAlreadyExists);

            var hashedPassword = _passwordHasher.Hash(command.Password);
            var passwordHash = PasswordHash.Create(hashedPassword);

            var user = User.Create(name, email, passwordHash);
            var response = new RegisterUserResponse(user.Id);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return Result<RegisterUserResponse>.Success(response);
        }

    }
}
