using Cashly.Api.Contracts.IdentityContext.LoginUser;
using Cashly.Application.IdentityContext.UseCases.loginUser;
using Cashly.Application.Shared.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.IdentityContext
{
    [ApiController]
    [Route("api/login")]
    public sealed class LoginController : ControllerBase
    {
        private readonly LoginUserHandler _handler;
        private readonly IValidator<LoginUserCommand> _validator;

        public LoginController(LoginUserHandler handler, IValidator<LoginUserCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginUserResquestDto request)
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => new
                {
                    property = error.PropertyName,
                    message = error.ErrorMessage,
                });

                return BadRequest(errors);
            }

            Result<LoginUserResponse> result = await _handler.Handle(command);

            if (result.IsFailure)
            {
                return BadRequest(new
                {
                    code = result.Error.Code,
                    message = result.Error.Message,
                });
            }

            var response = new LoginUserResponseDto(result.Value.AccessToken, result.Value.ExpiresAt);

            return Ok(response);

        }

    }
}
