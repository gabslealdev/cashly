using Cashly.Api.Contracts.IdentityContext.RegisterUser;
using Cashly.Application.IdentityContext.UseCases.RegisterUser;
using Cashly.Application.Shared.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.Identity
{
    [ApiController]
    [Route("api/identity/user")]
    public sealed class UsersController : ControllerBase
    {
        private readonly RegisterUserHandler _handler;
        private readonly IValidator<RegisterUserCommand> _validator;

        public UsersController(RegisterUserHandler handler, IValidator<RegisterUserCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }       

        [HttpPost]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            var command = new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password);

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

            Result<RegisterUserResponse> result = await _handler.HandleAsync(command);

            if (result.IsFailure)
            {
                return BadRequest(new 
                {
                    code = result.Error.Code,
                    message = result.Error.Message,
                });
            }

            var response = new RegisterUserResponseDto(result.Value.UserId);

            return Created(string.Empty, response);
        }
    }
}
