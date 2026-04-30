using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.IdentityContext.Interfaces.Security;
using Cashly.Application.IdentityContext.UseCases.loginUser.Errors;
using Cashly.Application.Shared.Results;
using Cashly.Domain.IdentityContext.ValueObjects;

namespace Cashly.Application.IdentityContext.UseCases.loginUser
{
    public sealed class LoginUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository; 
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand command)
        {
            var email = Email.Create(command.Email);

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
                return Result<LoginUserResponse>.Failure(LoginUserApplicationErrors.InvalidCredentials);

            var isPasswordValid = _passwordHasher.Verify(command.Password, user.PasswordHash);

            if (!isPasswordValid)
                return Result<LoginUserResponse>.Failure(LoginUserApplicationErrors.InvalidCredentials);

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email.Value);

            return Result<LoginUserResponse>.Success(new LoginUserResponse(token.AccessToken, token.ExpiresAt));

        }

    }
}
