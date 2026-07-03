using Cashly.Api.Contracts.Common.Errors;
using Cashly.Api.Contracts.IdentityContext.LoginUser;
using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.IdentityContext.UseCases.LoginUser;
using Cashly.Application.Shared.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.IdentityContext
{
    [ApiController]
    [Route("api/login")]
    public sealed class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<LoginUserCommand> _validator;

        public LoginController(IMediator mediator, IValidator<LoginUserCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto request, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error =>
                    new ApiError(
                        Code: $"Validation.{error.PropertyName}",
                        Message: error.ErrorMessage,
                        Property: error.PropertyName)).ToList();

                return BadRequest(new ErrorResponse(errors));
            }

            Result<LoginUserResponse> result = await _mediator.SendAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(new ErrorResponse([
                    new ApiError(result.Error.Code, result.Error.Message)
                ]));
            }

            var response = new LoginUserResponseDto(result.Value.AccessToken, result.Value.ExpiresAt);

            return Ok(response);

        }

    }
}
