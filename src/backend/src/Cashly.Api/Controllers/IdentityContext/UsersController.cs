using Cashly.Api.Contracts.Common.Errors;
using Cashly.Api.Contracts.IdentityContext.RegisterUser;
using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.IdentityContext.UseCases.RegisterUser;
using Cashly.Application.Shared.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.IdentityContext
{
    [ApiController]
    [Route("api/identity/user")]
    public sealed class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<RegisterUserCommand> _validator;

        public UsersController(IMediator mediator, IValidator<RegisterUserCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }       

        [HttpPost]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password);

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

            Result<RegisterUserResponse> result = await _mediator.SendAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return Conflict(new ErrorResponse([
                    new ApiError(result.Error.Code, result.Error.Message)
                ]));
            }

            var response = new RegisterUserResponseDto(result.Value.UserId);

            return Created(string.Empty, response);
        }
    }
}
