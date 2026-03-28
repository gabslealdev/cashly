using Cashly.Application.Identity.UseCases.loginUser;
using Cashly.Application.Shared.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.Identity
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
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
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

            return Ok(result.Value);

        }

    }
}
